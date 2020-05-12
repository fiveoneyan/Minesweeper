using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine;
namespace komal.sdk
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class IAPSDK : SDKBase, IIAP, IStoreListener
    {
        // Purchase State
        private enum PurchaseState {
            NONE,
            PURCHASING
        }

        private enum RestoreState {
            NONE,
            RESTORING,
            DONE
        }

        //////////////////////////////////////////////////////////////
        //// IIAP
        //////////////////////////////////////////////////////////////
        public void Purchase(string productKey)
        {
            if(!KomalUtil.Instance.IsNetworkReachability()){
                this.facade.SendNotification(IAP_MSG.PURCHASE_FAILURE);
                return;
            }

            komal.Config.ID.iap.ForEach(it=>{
                if(string.Equals(productKey, it.Key)){
                    // Buy the consumable product using its general identifier. Expect a response either 
                    // through ProcessPurchase or OnPurchaseFailed asynchronously.
                    BuyProductID(it.ID);
                }
            });
        }

        // only for nonconsumable product
        public bool IsPurchased(string productKey)
        {
            var productId = ReadPurchase(productKey);
            return !string.Equals(productId, "");
        }

        public void RemoveAds()
        {
            this.Purchase("remove_ads");
        }

        public bool IsAdsRemoved()
        {
            return this.IsPurchased("remove_ads");
        }
        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            if(!KomalUtil.Instance.IsNetworkReachability()){
                this.facade.SendNotification(IAP_MSG.PURCHASE_FAILURE);
                return;
            }

            if(m_RestoreState == RestoreState.RESTORING){
                this.facade.SendNotification(IAP_MSG.RESTORING);
            }else{
                // If Purchasing has not yet been set up ...
                if (IsInitialized())
                {
                    // If we are running on an Apple device ... 
                    if (Application.platform == RuntimePlatform.IPhonePlayer || 
                        Application.platform == RuntimePlatform.OSXPlayer)
                    {
                        // ... begin restoring purchases
                        Debug.Log("RestorePurchases started ...");
                        setLoadingPanel(true);
                        // Fetch the Apple store-specific subsystem.
                        var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                        m_RestoreState = RestoreState.RESTORING;
                        // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                        // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                        apple.RestoreTransactions((result) => {
                            // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                            // no purchases are available to be restored.
                            Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                            setLoadingPanel(false);
                            if (result) {
                                // This does not mean anything was restored,
                                // merely that the restoration process succeeded.
                                m_RestoreState = RestoreState.DONE;
                            } else {
                                // Restoration failed.
                                m_RestoreState = RestoreState.NONE;
                                this.facade.SendNotification(IAP_MSG.RESTORE_FAILURE);
                            }
                        });
                    }
                    // Otherwise ...
                    else
                    {
                        // We are not running on an Apple device. No work is necessary to restore purchases.
                        Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
                        this.facade.SendNotification(IAP_MSG.RESTORE_FAILURE);
                    }
                }else{
                    // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                    Debug.Log("RestorePurchases FAIL. Not initialized.");
                    this.facade.SendNotification(IAP_MSG.RESTORE_FAILURE);
                }
            }
        }

        public bool IsSupportRestorePurchases(){
            if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.OSXPlayer){
                return true;
            }else{
                return false;
            }
        }

        public bool IsPurchasing(){
            return m_PurchaseState == PurchaseState.PURCHASING;
        }

        private void BuyProductID(string productId)
        {
            // clean the restore state
            m_RestoreState = RestoreState.NONE;
            if(m_PurchaseState == PurchaseState.PURCHASING){
                this.facade.SendNotification(IAP_MSG.PURCHASING);
            }else{
                // If Purchasing has been initialized ...
                if (IsInitialized())
                {
                    // ... look up the Product reference with the general product identifier and the Purchasing 
                    // system's products collection.
                    Product product = m_StoreController.products.WithID(productId);

                    // If the look up found a product for this device's store and that product is ready to be sold ... 
                    if (product != null && product.availableToPurchase)
                    {
                        setLoadingPanel(true);
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                        // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                        // asynchronously.
                        m_PurchaseState = PurchaseState.PURCHASING;
                        m_StoreController.InitiatePurchase(product);
                    }
                    // Otherwise ...
                    else
                    {
                        // ... report the product look-up failure situation  
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                        this.facade.SendNotification(IAP_MSG.PURCHASE_FAILURE);
                    }
                }
                // Otherwise ...
                else
                {
                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                    // retrying initiailization.
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                    this.facade.SendNotification(IAP_MSG.PURCHASE_FAILURE);
                }
            }
        }


        //////////////////////////////////////////////////////////////
        //// ILifeCycle
        //////////////////////////////////////////////////////////////
        public override void OnInit() {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                // If we have already connected to Purchasing ...
                if (IsInitialized())
                {
                    // ... we are done here.
                    return;
                }
                m_Loading = false;

                // Create a builder, first passing in a suite of Unity provided stores.
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                komal.Config.ID.iap.ForEach(it=>{
                    if(it.Type == komal.Config.PurchaseType.Consumable){
                        // Add a product to sell / restore by way of its identifier, associating the general identifier
                        // with its store-specific identifiers.
                        builder.AddProduct(it.ID, ProductType.Consumable);
                    }else if(it.Type == komal.Config.PurchaseType.NonConsumable){
                        // Continue adding the non-consumable product.
                        builder.AddProduct(it.ID, ProductType.NonConsumable);
                    }
                });

                // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
                // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
                UnityPurchasing.Initialize(this, builder);
            }
        }

        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        //////////////////////////////////////////////////////////////
        //// IStoreListener
        //////////////////////////////////////////////////////////////
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("IAP >>>> OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
        {
            setLoadingPanel(false);
            m_PurchaseState = PurchaseState.NONE;
            Debug.Log(string.Format("SDK >> ProcessPurchase >> Product: '{0}'", args.purchasedProduct.definition.id));
            var productId = args.purchasedProduct.definition.id;
            var notifyBody = new IAPSuccessResult();
            notifyBody.product = args.purchasedProduct;
            bool isRestore = this.m_RestoreState == RestoreState.RESTORING;
            if(isRestore){
                this.m_RestoreState = RestoreState.DONE;
            }
            Config.ID.iap.ForEach(it=>{
                // do something with entry.Value or entry.Key
                if(string.Equals(it.ID, productId)){
                    notifyBody.productKey = it.Key;
                    notifyBody.productItem = it;
                    notifyBody.isRestore = isRestore;
                    if(it.Type == Config.PurchaseType.NonConsumable){
                        // record purchase
                        WritePurchase(it.Key, productId);
                    }
                }
            });
            this.facade.SendNotification(IAP_MSG.PURCHASE_SUCCESS, notifyBody);
            if(isRestore){
                this.facade.SendNotification(IAP_MSG.RESTORE_SUCCESS);
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            setLoadingPanel(false);
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
            m_PurchaseState = PurchaseState.NONE;
            this.facade.SendNotification(IAP_MSG.PURCHASE_FAILURE);
        }

        //////////////////////////////////////////////////////////////
        //// Properties getter && setters
        //////////////////////////////////////////////////////////////
        private void WritePurchase(string productKey, string productId){
            KomalUtil.Instance.SetItem(GetLocalStorageKey(productKey), productId);
        }
        private string ReadPurchase(string productKey){
            return KomalUtil.Instance.GetItem(GetLocalStorageKey(productKey), "");
        }
        private string GetLocalStorageKey(string key){ return "iap_" + key; }
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
        private PurchaseState m_PurchaseState = PurchaseState.NONE; // The purchase state.
        private RestoreState m_RestoreState = RestoreState.NONE;
        private bool m_Loading = false; // 真正的 loading 态
        private void setLoadingPanel(bool isShow){
            if(m_Loading != isShow){
                m_Loading = isShow;
                this.facade.SendNotification(m_Loading ? MSG_LOADING.ON : MSG_LOADING.OFF);
            }
        }
    }
}
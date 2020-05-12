using UnityEngine;
using komal.sdk;
using komal.puremvc;

namespace komal.test {
    public class TestIAP : ComponentEx 
    {
        public void Test_IAP_BuyNonConsumable(){
            // 以下两个是等价的
            // SDKManager.Instance.Purchase("remove_ads");
            SDKManager.Instance.RemoveAds();
        }

        public void Test_IAP_BuyConsumable(){
            // SDKManager.Instance.Purchase("100Coins");
            KomalUtil.Instance.ShowMessage("IAP", "No config for consumable items.");
        }

        public void Test_IAP_IsPurchased(){
            // 以下两个是等价的
            // SDKManager.Instance.IsAdsRemoved();
            var isPurchased = SDKManager.Instance.IsPurchased("remove_ads");
            KomalUtil.Instance.ShowMessage("IAP", isPurchased ? "remove_ads IsPurchased: yes" : "remove_ads IsPurchased: no");
        }

        public void Test_IAP_IsSupportRestore(){
            var isSupport = SDKManager.Instance.IsSupportRestorePurchases();
            KomalUtil.Instance.ShowMessage("IAP", isSupport ? "SupportRestore: yes" : "SupportRestore: no");
        }

        public void Test_IAP_RestorePurchases(){
            if(SDKManager.Instance.IsSupportRestorePurchases()){
                SDKManager.Instance.RestorePurchases();
            }
        }

        public override string[] ListNotificationInterests()
        {
            return new string[] {
                IAP_MSG.PURCHASING,
                IAP_MSG.PURCHASE_FAILURE,
                IAP_MSG.PURCHASE_SUCCESS,
                IAP_MSG.RESTORING,
                IAP_MSG.RESTORE_FAILURE,
                IAP_MSG.RESTORE_SUCCESS
            };
        }

        public override void HandleNotification(INotification notification)
        {
            var msgName = notification.name;
            switch (msgName)
            {
                case IAP_MSG.PURCHASING:
                    KomalUtil.Instance.ShowMessage("IAP", "It is purchasing!");
                    break;
                case IAP_MSG.PURCHASE_FAILURE:
                    KomalUtil.Instance.ShowMessage("IAP", "Purchase Failed!");
                    break;
                case IAP_MSG.PURCHASE_SUCCESS:
                    // 只要购买成功， NonComsumable 类型的产品的购买信息，会被 komal 框架记录下来。可以通过 
                    // SDKManager.Instance.IsPurchased("remove_ads"); 这样的接口获取状态；
                    IAPSuccessResult result = (IAPSuccessResult)notification.body;
                    var product = result.product; // Unity IAP 返回的产品数据结构；
                    var productKey = result.productKey; // 配置 IDConfig_iOS.json 中用于程序购买的关键字(remove_ads)
                    var productItem = result.productItem; 
                    KomalUtil.Instance.ShowMessage("IAP", "Purchase Success! product id:" + productItem.ID);
                    break;
                case IAP_MSG.RESTORING:
                    KomalUtil.Instance.ShowMessage("IAP", "It is restoring!");
                    break;
                case IAP_MSG.RESTORE_FAILURE:
                    KomalUtil.Instance.ShowMessage("IAP", "Restore Purchase Failed!");
                    break;
                case IAP_MSG.RESTORE_SUCCESS:
                    // restore 成功，物品信息购买的消息会通过 purechase 的部分派发（如果有购买过的话）
                    KomalUtil.Instance.ShowMessage("IAP", "Restore Purchase Success!");
                    break;
            }
        }
    }
}

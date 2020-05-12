/************************************************************************************
Author: Komal Zheng
Date: "2019-08-08"
Description: AppsFlyer SDK
************************************************************************************/

using System.Collections.Generic;
using komal.puremvc;

namespace komal.sdk
{
    public class AppsFlyerAnalyticsSDK : SDKBase, IAnalytics
    {
        public void SendBeginePageEvent(string pageName)
        {
            // 不处理；
        }

        public void SendEndPageEvent(string pageName)
        {
            // 不处理；
        }

        public void SendLaunchEvent()
        {
            // 不处理；
        }

        public void SendTerminateEvent()
        {
            // 不处理；
        }

        /*
            评级
            content_type: 评级的类型 "level"
            content_id: 例如 55 关则填 55
            rating_value: 评价值（0-不喜欢, 1-喜欢）
            max_rating_value: 评价值的最高分（填写 1）
         */
        public void SendRateEvent(string content_type, string content_id, string rating_value, string max_rating_value){
            Dictionary<string, string> eventDict = new Dictionary<string,string> ();
            eventDict.Add(AFInAppEvents.CONTENT_TYPE, content_type);
            eventDict.Add(AFInAppEvents.CONTENT_ID, content_id);
            eventDict.Add(AFInAppEvents.RATING_VALUE, rating_value);
            eventDict.Add(AFInAppEvents.MAX_RATING_VALUE, max_rating_value);
            AppsFlyer.trackRichEvent(AFInAppEvents.RATE, eventDict);
        }

        /*
            productId: 商品 ID
            contentType: 商品分类
            revenue: 收入，可以为负数
            currency: 货币类型
         */
        public void SendPurchaseEvent(string productId, string contentType, string revenue, string currency = "USD"){
            // 高级属性数据发送的开关
            if(this.m_isAdvancedToggleOn){
                Dictionary<string, string> eventDict = new Dictionary<string,string> ();
                eventDict.Add(AFInAppEvents.REVENUE, revenue);
                eventDict.Add(AFInAppEvents.CONTENT_TYPE, contentType);
                eventDict.Add(AFInAppEvents.CONTENT_ID, productId);
                eventDict.Add(AFInAppEvents.CURRENCY, currency);
                AppsFlyer.trackRichEvent(AFInAppEvents.PURCHASE, eventDict);
            }
        }

        public string GetDeviceID(){
            return AppsFlyer.getAppsFlyerId();
        }

        // 生命周期函数
        public override void OnInit() {
            /* Mandatory - set your AppsFlyer’s Developer key. */
            AppsFlyer.setAppsFlyerKey (Config.ID.GetValue("AppsFlyerKey"));
            /* For detailed logging */
            /* AppsFlyer.setIsDebug (true); */
            #if UNITY_IOS
            /* Mandatory - set your apple app ID
            NOTE: You should enter the number only and not the "ID" prefix */
            AppsFlyer.setAppID (Config.ID.GetValue("AppId"));
            AppsFlyer.trackAppLaunch ();
            // register to push notifications for iOS uninstall
		    UnityEngine.iOS.NotificationServices.RegisterForNotifications (UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
            #elif UNITY_ANDROID
            /* Mandatory - set your Android package name */
            AppsFlyer.setAppID (Config.ID.GetValue("PackageName"));
            /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
            AppsFlyer.init (Config.ID.GetValue("AppsFlyerKey"),"AppsFlyerTrackerCallbacks");
            #endif
            // 初始化高级属性数据发送的开关
            this.m_isAdvancedToggleOn = Config.ID.GetToggle("AppsFlyerAdvanced");

            this.SendLaunchEvent();
        }

        public override void OnDestroy() {
            this.SendTerminateEvent();
        }

        public override void OnUpdate() {
            base.OnUpdate();
            #if UNITY_IOS 
            if (!tokenSent) { 
                byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;           
                if (token != null) {     
                    //For iOS uninstall
                    AppsFlyer.registerUninstall (token);
                    tokenSent = true;
                }
            }
            #endif
        }

        // 监听并处理内购的事件
        public override string[] ListNotificationInterests()
        {
            return new string[] {
                IAP_MSG.PURCHASE_SUCCESS,
                MSG_GAMECENTER.GAMECENTER_LOGIN_SUCCESS
            };
        }

        public override void HandleNotification(INotification notification)
        {
            var msgName = notification.name;
            switch (msgName)
            {
                case IAP_MSG.PURCHASE_SUCCESS:
                    //////////////////////////////////////////////////////////////
                    //// 应用内事件：记录购买的产品 ///////////////////////////////////
                    //////////////////////////////////////////////////////////////
                    // 只要购买成功， NonComsumable 类型的产品的购买信息，会被 komal 框架记录下来。可以通过 
                    // SDKManager.Instance.IsPurchased("remove_ads"); 这样的接口获取状态；
                    IAPSuccessResult result = (IAPSuccessResult)notification.body;
                    var product = result.product; // Unity IAP 返回的产品数据结构；
                    var productKey = result.productKey; // 配置 IDConfig.cs 中用于程序购买的关键字(remove_ads)
                    var productItem = result.productItem; // 配置 IDConfig.cs 中，在 app store 上填写的信息；
                    // 不是恢复购买的情况下
                    if(!result.isRestore){
                        // 向 AppsFlyer 发送消息
                        this.SendPurchaseEvent(productItem.ID, 
                            productItem.Type, 
                            productItem.Price.ToString(), 
                            productItem.Currency);
                    }
                    break;
                case MSG_GAMECENTER.GAMECENTER_LOGIN_SUCCESS:
                    //////////////////////////////////////////////////////////////
                    //// 应用内事件：记录登陆成功事件 /////////////////////////////////
                    //////////////////////////////////////////////////////////////
                    AppsFlyer.trackRichEvent ("af_login", null);
                    break;
            }
        }

        //////////////////////////////////////////////////////////////
        //// 卸载事件追踪
        //////////////////////////////////////////////////////////////
        // 标记 AppsFlyer 是否已经发送了 Token
        #if UNITY_IOS
        private bool tokenSent = false;
        #endif
        //////////////////////////////////////////////////////////////
        //// 高级属性是否向 AppsFlyer 发送的开关
        //////////////////////////////////////////////////////////////
        private bool m_isAdvancedToggleOn = false;
    }
}

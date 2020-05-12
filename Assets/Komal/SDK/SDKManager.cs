using System;
using System.Collections.Generic;
using komal.puremvc;

namespace komal.sdk {
    public class SDKManager: Singleton<SDKManager>, ILifeCycle, IDebugLog, IAD,  IGameCenter, IIAP, IAnalytics, IPush
    {
        public override string SingletonName(){ return "SDKManager"; }
        public override void OnSingletonInit(){ this.OnInit(); }

        private List<SDKBase> m_SDKList = new List<SDKBase>();
        private bool m_isInitialized = false;
        private IDebugLog m_ProxyDebugLog = null;
        private IAD m_ProxyAD = null;
        private IGameCenter m_ProxyGameCenter = null;
        private IIAP m_ProxyIAP = null;
        private List<IAnalytics> m_AnalyticList = new List<IAnalytics>();
        private IPush m_ProxyPush = null; 

        ////////////////////////////////////////////////////////////////
        // 调试接口 IDebugLog 
        ////////////////////////////////////////////////////////////////
        public string GetLogFileFullPath(){ return m_ProxyDebugLog.GetLogFileFullPath(); }
        public string GetRunTimeLogText() { return m_ProxyDebugLog.GetRunTimeLogText(); }


        ////////////////////////////////////////////////////////////////
        // 广告接口 IAD
        ////////////////////////////////////////////////////////////////
        public void ShowBanner(){
            if(this.IsAdsRemoved()
            || (!KomalUtil.Instance.IsNetworkReachability())
            ){
                return;
            }
            m_ProxyAD.ShowBanner(); 
        }
        public void HideBanner(){ m_ProxyAD.HideBanner(); }
        public void ShowInterstitial(System.Action<InterstitialResult> callback){
            if( !IsInterstitialAvailable()
            || this.IsAdsRemoved() 
            || (!KomalUtil.Instance.IsNetworkReachability())
            ){
                if(callback!=null){ callback(InterstitialResult.UNAVAILABLE); } 
                return; 
            }
            m_ProxyAD.ShowInterstitial(callback); 
        }
        public void ShowRewardedVideo(System.Action<RewardedVideoResult> callback){ 
            if( !IsRewardedVideoAvailable()
            || (!KomalUtil.Instance.IsNetworkReachability())
            ){
                if(callback!=null){ callback(RewardedVideoResult.UNAVAILABLE); } 
                return;
            }
            m_ProxyAD.ShowRewardedVideo(callback); 
        }
        public bool IsDisplayingAD(){ return m_ProxyAD.IsDisplayingAD(); }
        public bool IsBannerAvailable(){ return m_ProxyAD.IsBannerAvailable(); }
        public bool IsRewardedVideoAvailable(){ return m_ProxyAD.IsRewardedVideoAvailable(); }
        public bool IsInterstitialAvailable(){ return m_ProxyAD.IsInterstitialAvailable(); }
        public void ValidateIntegration() { m_ProxyAD.ValidateIntegration(); }

        ////////////////////////////////////////////////////////////////
        // 游戏中心接口 IGameCenter
        ////////////////////////////////////////////////////////////////
        public void Login(Action<bool, string> callback = null) {
            m_ProxyGameCenter.Login((success, info)=>{
                // 派发登陆成功消息
                if(callback != null){ callback(success, info); }
                if(success){
                    this.facade.SendNotification(MSG_GAMECENTER.GAMECENTER_LOGIN_SUCCESS, info);
                }
            }); 
        }
        public bool IsAuthenticated() { return m_ProxyGameCenter.IsAuthenticated(); }
        public void OpenGameCenter() { m_ProxyGameCenter.OpenGameCenter(); }
        public void RecordRank(string key, int value, System.Action<bool> callback = null) { m_ProxyGameCenter.RecordRank(key, value, callback); }
        public void RecordAchievement(string key, double value, System.Action<bool> callback = null) { m_ProxyGameCenter.RecordAchievement(key, value, callback); }
        public void FiveStars(){ m_ProxyGameCenter.FiveStars(); }

        ////////////////////////////////////////////////////////////////
        // 内置购买接口 IIAP
        ////////////////////////////////////////////////////////////////
        public void Purchase(string productKey){
            if(m_ProxyIAP != null){
                m_ProxyIAP.Purchase(productKey);
            }
        }
        public bool IsPurchased(string productKey) { 
            if(m_ProxyIAP != null){
                return m_ProxyIAP.IsPurchased(productKey); 
            }else{
                return false;
            }
        }
        public void RemoveAds() {
            if(m_ProxyIAP != null){
                m_ProxyIAP.RemoveAds();
            } 
        }
        public bool IsAdsRemoved() {
            if(m_ProxyIAP != null){
                return m_ProxyIAP.IsAdsRemoved(); 
            }else{
                return false;
            }
        }
        public void RestorePurchases(){
            if(m_ProxyIAP != null){
                m_ProxyIAP.RestorePurchases();
            }
        }
        public bool IsSupportRestorePurchases(){
            if(m_ProxyIAP != null){
                return m_ProxyIAP.IsSupportRestorePurchases();
            }else{
                return false;
            }
        }
        public bool IsPurchasing(){ 
            if(m_ProxyIAP != null){
                return m_ProxyIAP.IsPurchasing(); 
            }else{
                return false;
            }
        }

        ////////////////////////////////////////////////////////////////
        // 统计分析接口 IAnalytics
        ////////////////////////////////////////////////////////////////
        public void SendLaunchEvent() { m_AnalyticList.ForEach(analytic => analytic.SendLaunchEvent()); }
        public void SendTerminateEvent() { m_AnalyticList.ForEach(analytic => analytic.SendTerminateEvent()); }
        public void SendBeginePageEvent(string pageName) { m_AnalyticList.ForEach(analytic => analytic.SendBeginePageEvent(pageName)); }
        public void SendEndPageEvent(string pageName) { m_AnalyticList.ForEach(analytic => analytic.SendEndPageEvent(pageName)); }
        /*  评级
            content_type: 评级的类型 "level"
            content_id: 例如 55 关则填 55
            rating_value: 评价值（0-不喜欢, 1-喜欢）
            max_rating_value: 评价值的最高分（填写 1）
         */
        public void SendRateEvent(string content_type, string content_id, string rating_value, string max_rating_value){ m_AnalyticList.ForEach(analytic => analytic.SendRateEvent(content_type, content_id, rating_value, max_rating_value)); }
        //////////////////////////////////////////////////////////////
        //// 推送接口
        //////////////////////////////////////////////////////////////
        public void NotificationMessage(string message, DateTime newDate, bool isRepeatDay) {if(m_ProxyPush != null){ m_ProxyPush.NotificationMessage(message, newDate, isRepeatDay); }}

        ////////////////////////////////////////////////////////////////
        // 生命周期接口 ILifeCycle
        ////////////////////////////////////////////////////////////////
        public void OnInit()
        {
            // load all sdk
            if(!m_isInitialized){
                m_isInitialized = true;

                // 日志记录模块
                var debugLogSDK = new DebugLogSDK();
                m_SDKList.Add(debugLogSDK);
                this.m_ProxyDebugLog = debugLogSDK;

                // 广告模块
#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                var ironSourceSDK = new IronSourceSDK();
                m_SDKList.Add(ironSourceSDK);
                m_ProxyAD = ironSourceSDK;
#else
                var adSimulatorSDK = new ADSimulatorSDK();
                m_SDKList.Add(adSimulatorSDK);
                m_ProxyAD = adSimulatorSDK;
#endif

#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
            // iOS GameCenter SDK
                var _iOSGameCenter = new iOSGameCenter();
                m_SDKList.Add(_iOSGameCenter);
                this.m_ProxyGameCenter = _iOSGameCenter;
#elif UNITY_ANDROID && !UNITY_EDITOR
                // android GameCenter SDK
                var _androidGameCenter = new AndroidGameCenter();
                m_SDKList.Add(_androidGameCenter);
                this.m_ProxyGameCenter = _androidGameCenter;
#else
                // simulator GameCenter SDK
                var _simulatorGameCenter = new SimulatorGameCenter();
                m_SDKList.Add(_simulatorGameCenter);
                this.m_ProxyGameCenter = _simulatorGameCenter;
#endif

#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                // IAP
                var iapSDK = new IAPSDK();
                m_SDKList.Add(iapSDK);
                this.m_ProxyIAP = iapSDK;
#endif

                // 推送
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                var pushToggle = Config.ID.GetToggle("Push");
                if(pushToggle){
                    var pushSDK = new iOSPush();
                    m_SDKList.Add(pushSDK);
                    m_ProxyPush = pushSDK;
                }
#endif
                // IAnalytics
#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                // AppsFlyer SDK
                var appsFlyerToggle = Config.ID.GetValue("AppsFlyerKey");
                if(!(appsFlyerToggle == null || appsFlyerToggle == "")){
                    var appsFlyerAnalyticsSDK = new AppsFlyerAnalyticsSDK();
                    m_SDKList.Add(appsFlyerAnalyticsSDK);
                    m_AnalyticList.Add(appsFlyerAnalyticsSDK);
                }
#endif


                m_SDKList.ForEach(sdk=>{
                    sdk.OnInit();
                });
            }
        }

        public void OnPause() { m_SDKList.ForEach(sdk=>sdk.OnPause()); }
        public void OnResume() { m_SDKList.ForEach(sdk=>sdk.OnResume()); }
        public void OnDestroy() { m_SDKList.ForEach(sdk=>sdk.OnDestroy()); }
        public void OnUpdate(){ m_SDKList.ForEach(sdk=>sdk.OnUpdate()); }

        public override string[] ListNotificationInterests()
        {
            return new string[]{
                IAP_MSG.PURCHASE_SUCCESS
            };
        }

        public override void HandleNotification(INotification notification)
        {
            var msgName = notification.name;
            switch (msgName)
            {
                case IAP_MSG.PURCHASE_SUCCESS:
                    // 只要购买成功， NonComsumable 类型的产品的购买信息，会被 komal 框架记录下来。可以通过 
                    // SDKManager.Instance.IsPurchased("remove_ads"); 这样的接口获取状态；
                    IAPSuccessResult result = (IAPSuccessResult)notification.body;
                    var product = result.product; // Unity IAP 返回的产品数据结构；
                    var productKey = result.productKey; // 配置 IDConfig_iOS.json 中用于程序购买的关键字(remove_ads)
                    var productItem = result.productItem; 
                    if(productItem.Key == "remove_ads"){
                        if(this.IsAdsRemoved()){
                            // 隐藏 Banner 广告
                            this.HideBanner();
                        }
                    }
                    break;
            }
        }
    }
}

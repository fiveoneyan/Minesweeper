/* Brief: IronSource wrap SDK
 * Author: Komal
 * Date: "2019-07-09"
 * Note: 
 *     1. load interstitial on init and every time interstitial closed.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace komal.sdk {
    public class IronSourceSDK : SDKBase, IAD
    {
        enum ADStatus {
            NotReady,
            Loading,
            Ready,
            Playing
        }
        private ADStatus m_BannerStatus = ADStatus.NotReady;
        private ADStatus m_InterstitialStatus = ADStatus.NotReady;
        private ADStatus m_RewardedVideoStatus = ADStatus.NotReady;
        private bool m_isRewarded = false;
        private bool m_isInterstitialAdClicked = false;
        private bool m_showBannerAfterLoaded = false;
        private Action<InterstitialResult> m_InterstitialHandler = null; 
        private Action<RewardedVideoResult> m_RewardedVideoHandler = null;
        private Queue<Action> m_DelayCallQueue = new Queue<Action>();
        private void InvokeInterstitialHandlerAndClean(InterstitialResult result, bool clean = false){ 
            if(m_InterstitialHandler != null){
                var func = m_InterstitialHandler;
                if(clean){ m_InterstitialHandler = null; }
                func( result ); 
            }
        }
        private void InvokeRewardedVideoHandlerAndClean(RewardedVideoResult result, bool clean = false){
            if(m_RewardedVideoHandler != null){ 
                var func = m_RewardedVideoHandler;
                if(clean){ m_RewardedVideoHandler = null; }
                func( result ); 
            }
        }

#region public API && IAD interface
        public void ShowBanner() {
            Debug.Log("SDK:IronSourceSDK >>>> ShowBanner");
            if(m_BannerStatus == ADStatus.Ready){
                this.DisplayBanner(true);
            }else if(m_BannerStatus == ADStatus.NotReady){
                m_showBannerAfterLoaded = true;
                this.LoadBanner();
            }else if(m_BannerStatus == ADStatus.Loading){
                m_showBannerAfterLoaded = true;
            }
        }

        public void HideBanner() {
            Debug.Log("SDK:IronSourceSDK >>>> HideBanner");
            m_showBannerAfterLoaded = false;
            if(m_BannerStatus == ADStatus.Ready){
                this.DisplayBanner(false);
            }
        }

        public void ShowInterstitial(Action<InterstitialResult> callback) {
            Debug.Log("SDK:IronSourceSDK >>>> ShowInterstitial");
            m_InterstitialHandler = callback;
            if(m_RewardedVideoStatus == ADStatus.Playing || SDKManager.Instance.IsPurchasing()){
                this.InvokeInterstitialHandlerAndClean(InterstitialResult.UNAVAILABLE, true);
                return;
            }
            if(m_InterstitialStatus == ADStatus.Ready){
                this.DisplayInterstitial();
            }else if(m_InterstitialStatus == ADStatus.NotReady){
                this.LoadInterstitial();
                this.InvokeInterstitialHandlerAndClean(InterstitialResult.UNAVAILABLE, true);
            }
        }

        public void ShowRewardedVideo(Action<RewardedVideoResult> callback) {
            Debug.Log("SDK:IronSourceSDK >>>> ShowRewardedVideo");
            m_RewardedVideoHandler = callback;
            if(m_InterstitialStatus == ADStatus.Playing || SDKManager.Instance.IsPurchasing()){
                Debug.Log("Interstitial is playing, can't show RewardedVideo");
                this.InvokeRewardedVideoHandlerAndClean(RewardedVideoResult.UNAVAILABLE, true);
                return;
            }
            if(m_RewardedVideoStatus == ADStatus.Ready){
                Debug.Log("ShowRewardedVideo >>>> available");
                IronSource.Agent.showRewardedVideo();
            }else{
                Debug.Log("ShowRewardedVideo >>>> unavailable");
                this.InvokeRewardedVideoHandlerAndClean(RewardedVideoResult.UNAVAILABLE, true);
            }
        }

        public bool IsDisplayingAD(){
            return m_InterstitialStatus == ADStatus.Playing || m_RewardedVideoStatus == ADStatus.Playing;
        }

        public bool IsBannerAvailable(){
            return m_BannerStatus == ADStatus.Ready;
        }

        public bool IsRewardedVideoAvailable(){
            return m_RewardedVideoStatus == ADStatus.Ready;
        }

        public bool IsInterstitialAvailable(){
            return m_InterstitialStatus == ADStatus.Ready;
        }

        public void ValidateIntegration() {
            Debug.Log("SDK:IronSourceSDK >>>> ValidateIntegration");
            IronSource.Agent.validateIntegration();
        }
#endregion
        

#region private API
        private void DisplayBanner(bool bDisplay){
            Debug.Log("SDK:IronSourceSDK >>>> DisplayBanner");
            if(bDisplay){ 
                IronSource.Agent.displayBanner(); 
            }else{
                IronSource.Agent.hideBanner();
            } 
        }

        private void LoadBanner(){
            Debug.Log("SDK:IronSourceSDK >>>> LoadBanner");
            if(m_BannerStatus == ADStatus.NotReady){
                m_BannerStatus = ADStatus.Loading;
                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
                // IronSource.Agent.loadBanner(new IronSourceBannerSize(320, 50), IronSourceBannerPosition.BOTTOM);
            }
        }

        // A destroyed banner can no longer be loaded. If you want to serve it again, you must initiate it again.
        private void DestroyBanner(){
            Debug.Log("SDK:IronSourceSDK >>>> DestroyBanner");
            IronSource.Agent.destroyBanner();
            // TODO status must be changed
            m_BannerStatus = ADStatus.NotReady;
        }

        private void LoadInterstitial(){
            Debug.Log("SDK:IronSourceSDK >>>> LoadInterstitial");
            if(m_InterstitialStatus == ADStatus.NotReady){
                m_InterstitialStatus = ADStatus.Loading;
                IronSource.Agent.loadInterstitial();
            }
        }

        private void DisplayInterstitial(){
            Debug.Log("SDK:IronSourceSDK >>>> DisplayInterstitial");
            IronSource.Agent.showInterstitial();
        }

        // private void ShowRewardedVideo(string placementName, Action<RewardedVideoResult> callback){
        //     Debug.Log("SDK:IronSourceSDK >>>> ShowRewardedVideo placementName:" + placementName);

        //     IronSourcePlacement placement = IronSource.Agent.getPlacementInfo(placementName);
        //     //Placement can return null if the placementName is not valid.
        //     if(placement != null)
        //     {
        //         if(IronSource.Agent.isRewardedVideoPlacementCapped(placementName)){
        //             // placement uses capping & pacing
        //             Debug.Log("SDK:IronSourceSDK >>>> ShowRewardedVideo placement use capping & pacing");
        //         }else{
        //             Debug.Log("SDK:IronSourceSDK >>>> ShowRewardedVideo placement doesn't use capping & pacing");
        //             IronSource.Agent.showRewardedVideo(placementName);
        //         }
        //     }else{
        //         Debug.Log("SDK:IronSourceSDK >>>> ShowRewardedVideo placementName's info is null.");
        //     }
        // }

        // private void ShowInterstitial(string placementName, Action<InterstitialResult> callback){
        //     Debug.Log("SDK:IronSourceSDK >>>> ShowInterstitial placementName:" + placementName);

        //     IronSourcePlacement placement = IronSource.Agent.getPlacementInfo(placementName);
        //     //Placement can return null if the placementName is not valid.
        //     if(placement != null)
        //     {
        //         if(IronSource.Agent.isInterstitialPlacementCapped(placementName)){
        //             // placement uses capping & pacing
        //             Debug.Log("SDK:IronSourceSDK >>>> ShowInterstitial placement use capping & pacing");
        //         }else{
        //             Debug.Log("SDK:IronSourceSDK >>>> ShowInterstitial placement doesn't use capping & pacing");
        //             IronSource.Agent.showInterstitial(placementName);
        //         }
        //     }else{
        //         Debug.Log("SDK:IronSourceSDK >>>> ShowInterstitial placementName's info is null.");
        //     }
        // }
#endregion


#region ILifeCycle interface
        public override void OnInit(){
            Debug.Log("SDK:IronSourceSDK >>>> OnInit");
            var appKey = komal.Config.ID.GetValue("IronSourceAppKey");
            // For Banners
            IronSource.Agent.init (appKey, IronSourceAdUnits.BANNER);
            // For Rewarded Video
            IronSource.Agent.init (appKey, IronSourceAdUnits.REWARDED_VIDEO);
            // For Interstitial
            IronSource.Agent.init (appKey, IronSourceAdUnits.INTERSTITIAL);
            // // For Offerwall
            // IronSource.Agent.init (appKey, IronSourceAdUnits.OFFERWALL);
            // Track Network State

            // Regist Banner AD Events
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;        
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent; 
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent; 
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

            // Regist RewardedVideo AD Events
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent; 
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent; 
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

            // Register Interstitial AD Events
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;        
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent; 
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent; 
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            this.LoadInterstitial();
        }

        public override void OnPause(){
            Debug.Log("SDK:IronSourceSDK >>>> OnPause");
            IronSource.Agent.onApplicationPause(true);
        }

        public override void OnResume(){
            Debug.Log("SDK:IronSourceSDK >>>> OnResume");
            IronSource.Agent.onApplicationPause(false);
        }

        public override void OnDestroy(){
            Debug.Log("SDK:IronSourceSDK >>>> OnDestroy");
        }

        public override void OnUpdate(){
            base.OnUpdate();
            // 特殊处理，广告模块中，奖励会在关闭页面之后才有奖励通知；
            while(m_DelayCallQueue.Count > 0){
                var delayCall = m_DelayCallQueue.Dequeue();
                delayCall();
            }
        }
#endregion


#region IronSourceEvents Handler
        private class ERROR_CODE{
            // Load Fail
            public static readonly int INTERSTITIAL_LOAD_FAIL_NO_INTERNET_CONNECTION = 520;
            public static readonly int INTERSTITIAL_LOAD_FAIL_SERVER_RESPONSE_FAILED = 510;
            public static readonly int INTERSTITIAL_LOAD_FAIL_ADAPTERS_LOADING_FAILURE = 526;

            // Show Fail
            public static readonly int ALL_SHOW_FAIL_NO_ADS_TO_SHOW = 509;
            public static readonly int ALL_SHOW_FAIL_NO_INTERNET_CONNECTION = 520;
            public static readonly int ALL_SHOW_FAIL_PLACEMENT_HAS_REACHED_ITS_LIMIT_AS_DEFINED_PER_PACE = 524;
            public static readonly int REWARDEDVIDEO_SHOW_FAIL_AD_UNIT_HAS_REACHED_ITS_DAILY_CAP_PER_SESSION = 526;
        }
        //Invoked once the banner has loaded
        void BannerAdLoadedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> BannerAdLoadedEvent");
            if(m_BannerStatus == ADStatus.Loading){
                m_BannerStatus = ADStatus.Ready;
                this.DisplayBanner(m_showBannerAfterLoaded);
            }
        }
        //Invoked when the banner loading process has failed.
        //@param description - string - contains information about the failure.
        void BannerAdLoadFailedEvent (IronSourceError error) {
            Debug.Log("SDK:IronSourceSDK >>>> BannerAdLoadFailedEvent error:" + error.ToString());
            Assert.IsTrue( m_BannerStatus == ADStatus.Loading );
            m_BannerStatus = ADStatus.NotReady;
            // BannerAdLoadFailedEvent error:606 : No ads to show
        }
        // Invoked when end user clicks on the banner ad
        void BannerAdClickedEvent () {
            Debug.Log("SDK:IronSourceSDK >>>> BannerAdClickedEvent");
        }
        //Notifies the presentation of a full screen content following user click
        void BannerAdScreenPresentedEvent () {
            Debug.Log("SDK:IronSourceSDK >>>> BannerAdScreenPresentedEvent");
        }
        //Notifies the presented screen has been dismissed
        void BannerAdScreenDismissedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> BannerAdScreenDismissedEvent");
        }
        //Invoked when the user leaves the app
        void BannerAdLeftApplicationEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> BannerAdLeftApplicationEvent");
        }


        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        void RewardedVideoAdOpenedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAdOpenedEvent");
            m_RewardedVideoStatus = ADStatus.Playing;
            m_isRewarded = false;
            this.InvokeRewardedVideoHandlerAndClean(RewardedVideoResult.DISPLAY);
        }  
        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        void RewardedVideoAdClosedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAdClosedEvent");
            m_DelayCallQueue.Enqueue(()=>{
                m_RewardedVideoStatus = IronSource.Agent.isRewardedVideoAvailable() ? ADStatus.Ready : ADStatus.NotReady;
                this.InvokeRewardedVideoHandlerAndClean(RewardedVideoResult.DISMISS, true);
            });
        }
        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        void RewardedVideoAvailabilityChangedEvent(bool available) {
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAvailabilityChangedEvent available:" + available.ToString());
            //Change the in-app 'Traffic Driver' state according to availability.
            bool rewardedVideoAvailability = available;
            m_RewardedVideoStatus = available ? ADStatus.Ready : ADStatus.NotReady; 
        }
        //  Note: the events below are not available for all supported rewarded video 
        //   ad networks. Check which events are available per ad network you choose 
        //   to include in your build.
        //   We recommend only using events which register to ALL ad networks you 
        //   include in your build.
        //Invoked when the video ad starts playing.
        void RewardedVideoAdStartedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAdStartedEvent");
        }
        //Invoked when the video ad finishes playing.
        void RewardedVideoAdEndedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAdEndedEvent");
        }
        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for the callback from the  ironSource server.
        //
        //@param - placement - placement object which contains the reward data
        //
        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement) {
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAdRewardedEvent placement:" + placement.ToString());
            //Placement can return null if the placementName is not valid.
            if(placement != null)
            {
                String placementName = placement.getPlacementName();
                String rewardName = placement.getRewardName();
                int rewardAmount = placement.getRewardAmount();
                m_isRewarded = true;
                this.InvokeRewardedVideoHandlerAndClean(RewardedVideoResult.REWARDED);
            }
        }
        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        void RewardedVideoAdShowFailedEvent (IronSourceError error){
            Debug.Log("SDK:IronSourceSDK >>>> RewardedVideoAdShowFailedEvent error:" + error.ToString());
            m_RewardedVideoStatus = IronSource.Agent.isRewardedVideoAvailable() ? ADStatus.Ready : ADStatus.NotReady; 
            Debug.Log("SDK: m_RewardedVideoStatus >>>> " + m_RewardedVideoStatus.ToString());
            this.InvokeRewardedVideoHandlerAndClean(RewardedVideoResult.UNAVAILABLE, true);
        }


        //Invoked when the initialization process has failed.
        //@param description - string - contains information about the failure.
        void InterstitialAdLoadFailedEvent (IronSourceError error) {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdLoadFailedEvent error:" + error.ToString());
            this.m_InterstitialStatus = ADStatus.NotReady;
        }
        //Invoked right before the Interstitial screen is about to open.
        void InterstitialAdShowSucceededEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdShowSucceededEvent");
            this.m_InterstitialStatus = ADStatus.Playing;
            m_isInterstitialAdClicked = false;
            this.InvokeInterstitialHandlerAndClean(InterstitialResult.DISPLAY);
        }
        //Invoked when the ad fails to show.
        //@param description - string - contains information about the failure.
        void InterstitialAdShowFailedEvent(IronSourceError error) {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdShowFailedEvent error:" + error.ToString());
            this.m_InterstitialStatus = IronSource.Agent.isInterstitialReady() ? ADStatus.Ready : ADStatus.NotReady;
        }
        // Invoked when end user clicked on the interstitial ad
        void InterstitialAdClickedEvent () {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdClickedEvent");
            m_isInterstitialAdClicked = true;
        }
        //Invoked when the interstitial ad closed and the user goes back to the application screen.
        void InterstitialAdClosedEvent () {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdClosedEvent");
            // Once the InterstitialAdClosedEvent function is fired, you will be able to load a new Interstitial ad.
            m_InterstitialStatus = IronSource.Agent.isInterstitialReady() ? ADStatus.Ready : ADStatus.NotReady;
            if(this.m_InterstitialHandler != null){
                var func = this.m_InterstitialHandler;
                this.m_InterstitialHandler = null;
                this.LoadInterstitial();
                func(InterstitialResult.DISMISS);
            }else{
                this.LoadInterstitial();
            }
        }
        //Invoked when the Interstitial is Ready to shown after load function is called
        void InterstitialAdReadyEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdReadyEvent");
            Assert.IsTrue(m_InterstitialStatus == ADStatus.Loading);
            m_InterstitialStatus = ADStatus.Ready;
        }
        //Invoked when the Interstitial Ad Unit has opened
        void InterstitialAdOpenedEvent() {
            Debug.Log("SDK:IronSourceSDK >>>> InterstitialAdOpenedEvent");
        }
#endregion
    }
}

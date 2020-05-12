namespace komal.sdk {
    public interface IAD
    {
        void ShowBanner();

        void HideBanner();

        void ShowInterstitial(System.Action<InterstitialResult> callback);

        void ShowRewardedVideo(System.Action<RewardedVideoResult> callback);

        bool IsDisplayingAD();

        bool IsBannerAvailable();

        bool IsRewardedVideoAvailable();

        bool IsInterstitialAvailable();

        void ValidateIntegration();
    }
}

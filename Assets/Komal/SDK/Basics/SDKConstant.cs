namespace komal.sdk {
    public static class MSG_GAMECENTER {
        public const string GAMECENTER_LOGIN_SUCCESS = "GAMECENTER_LOGIN_SUCCESS";
    }
    
    public enum RewardedVideoResult {
        // RewardedVideo
        UNAVAILABLE,
        DISPLAY,
        REWARDED,
        DISMISS,
    }

    public enum InterstitialResult {
        // Interstitial
        UNAVAILABLE,
        DISPLAY,
        DISMISS
    }

    public static class IAP_MSG {
        public const string RESTORING = "IAP_MSG_RESTORING";
        public const string RESTORE_FAILURE = "IAP_MSG_RESTORE_FAILURE";
        public const string RESTORE_SUCCESS = "IAP_MSG_RESTORE_SUCCESS";
        public const string PURCHASING = "IAP_MSG_PURCHASING";
        public const string PURCHASE_FAILURE = "IAP_MSG_PURCHASE_FAILURE";
        public const string PURCHASE_SUCCESS = "IAP_MSG_PURCHASE_SUCCESS"; // only success will pass the product info.
    }

    // 购买成功时返回给界面的数据类型
    public class IAPSuccessResult{
        public UnityEngine.Purchasing.Product product = null;
        public Config.PurchaseItem productItem = null;
        public string productKey = null;
        public bool isRestore = false;
    }
}
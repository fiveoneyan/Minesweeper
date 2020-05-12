using UnityEngine;

namespace komal.test {
    public partial class TestKomalUtil
    {
        #region TapEngine
        public void Test_TapEngine_Notification(){
            KomalUtil.Instance.ShowRateUs("TapEngine Notification", "Select Type of Notification.", "SUCCESS", "WARNING", "ERROR", (NativeMessageType msgType)=>{
                switch (msgType)
                {
                    case NativeMessageType.RATE:
                        KomalUtil.Instance.TapEngineNotification(TapEngineNotificationType.SUCCESS);
                        break;
                    case NativeMessageType.REMIND:
                        KomalUtil.Instance.TapEngineNotification(TapEngineNotificationType.WARNING);
                        break;
                    case NativeMessageType.DECLINED:
                        KomalUtil.Instance.TapEngineNotification(TapEngineNotificationType.ERROR);
                        break;
                }
            });
        }

        public void Test_TapEngine_Selection(){
            KomalUtil.Instance.TapEngineSelection();
        }

        public void Test_TapEngine_Impact(){
            KomalUtil.Instance.ShowRateUs("TapEngine Impact", "Select Type of Impact.", "LIGHT", "MIDIUM", "HEAVY", (NativeMessageType msgType)=>{
                switch (msgType)
                {
                    case NativeMessageType.RATE:
                        KomalUtil.Instance.TapEngineImpact(TapEngineImpactType.LIGHT);
                        break;
                    case NativeMessageType.REMIND:
                        KomalUtil.Instance.TapEngineImpact(TapEngineImpactType.MIDIUM);
                        break;
                    case NativeMessageType.DECLINED:
                        KomalUtil.Instance.TapEngineImpact(TapEngineImpactType.HEAVY);
                        break;
                }
            });
        }

        public void Test_IsSupportTapEngine(){
            var isSupport = KomalUtil.Instance.IsSupportTapEngine();
            KomalUtil.Instance.ShowMessage("title:", isSupport ? "TapEngine is Supported!" : "TapEngine is not Supported!");
        }
        #endregion
    }
}
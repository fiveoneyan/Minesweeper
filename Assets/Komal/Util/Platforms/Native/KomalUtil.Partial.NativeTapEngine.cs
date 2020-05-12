using UnityEngine;
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace komal {
    public enum TapEngineNotificationType {
        SUCCESS = 0,
        WARNING = 1,
        ERROR = 2
    }

    public enum TapEngineImpactType {
        LIGHT = 0,
        MIDIUM = 1,
        HEAVY = 2
    }
    public partial class KomalUtil
    {
        public void TapEngineNotification(TapEngineNotificationType typ){
            TapEngine.Notification(typ);
        }

        public void TapEngineSelection(){
            TapEngine.Selection();
        }

        public void TapEngineImpact(TapEngineImpactType style){
            TapEngine.Impact(style);
        }

        public bool IsSupportTapEngine(){
            return TapEngine.IsSupport();
        }

#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _TAG_Unity_iOSTapticNotification(int typ);

        [DllImport("__Internal")]
        private static extern void _TAG_Unity_iOSTapticSelection();

        [DllImport("__Internal")]
        private static extern void _TAG_Unity_iOSTapticImpact(int style);

        [DllImport("__Internal")]
        private static extern bool _TAG_Unity_iOSTapticIsSupport();
#endif

        private class TapEngine {
            public static void Notification(TapEngineNotificationType typ){
                #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_Unity_iOSTapticNotification((int)typ);
                #endif
            }

            public static void Selection(){
                #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_Unity_iOSTapticSelection();
                #endif
            }

            public static void Impact(TapEngineImpactType style){
                #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_Unity_iOSTapticImpact((int)style);
                #endif
            }

            public static bool IsSupport(){
                #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                return _TAG_Unity_iOSTapticIsSupport();
                #else
                return false;
                #endif
            }
        }
    }
}

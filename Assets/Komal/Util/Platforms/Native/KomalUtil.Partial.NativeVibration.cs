/* Brief: Vibration
 * Author: Komal
 * Date: "2019-07-14"
 */

using UnityEngine;
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace komal {
    public enum VibrateType {
        NORMAL,
        PEEK,
        POP,
        CONTINUE
    }
    public interface IVibrate
    {
        void Vibrate(VibrateType typ);
        bool IsVibrateEnabled();
        void SetVibrateEnabled(bool isEnabled);
    }

    public partial class KomalUtil: IVibrate
    {
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _TAG_iOSNativeVibrate_Vibrate(string typ);
#endif

        public void Vibrate(VibrateType typ){
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
            if(typ == VibrateType.NORMAL){
                _TAG_iOSNativeVibrate_Vibrate("NORMAL");
            }else if(typ == VibrateType.PEEK){
                _TAG_iOSNativeVibrate_Vibrate("PEEK");
            }else if(typ == VibrateType.POP){
                _TAG_iOSNativeVibrate_Vibrate("POP");
            }else if(typ == VibrateType.CONTINUE){
                _TAG_iOSNativeVibrate_Vibrate("CONTINUE");
            }
#endif
        }

        public bool IsVibrateEnabled(){
            // 从本地配置中读取
            return KomalUtil.Instance.GetItem(KEY_VIBRATE, true);
        }

        public void SetVibrateEnabled(bool isEnabled){
            // 写入到本地配置
            KomalUtil.Instance.SetItem(KEY_VIBRATE, isEnabled);
        }

        static private string KEY_VIBRATE = "_vibrate_";
    }
}

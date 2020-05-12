/* Brief: NativeUrl Util
 * Author: Komal
 * Date: "2019-07-14"
 */

using UnityEngine;
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif


namespace komal {
// implementation for native contect
    interface INativeUrl {
        void OpenURL(string url);
    }

    public partial class KomalUtil: INativeUrl
    {
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
            [DllImport("__Internal")]
            private static extern void _TAG_iOSNativeURL_OpenURL(string url);
#endif

        public void OpenURL(string url){
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
            _TAG_iOSNativeURL_OpenURL(url);
#endif
        }
    }
}
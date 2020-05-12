using UnityEngine;

namespace komal
{
    public partial class KomalUtil {
        /// <summary>
        /// 网络可达性
        /// </summary> 
        /// <returns></returns>
        public bool IsNetworkReachability()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    // Debug.Log("当前使用的是：WiFi，请放心更新！");
                    return true;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    // Debug.Log("当前使用的是移动网络，是否继续更新？");
                    return true;
                default:
                    // Debug.Log("当前没有联网，请您先联网后再进行操作！");
                    return false;
            }
        }
    }
}

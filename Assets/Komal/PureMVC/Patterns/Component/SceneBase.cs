/* Brief: All Unity Scene must add SceneBase based Component, for all SDK lifecycle
 * Author: Komal
 * Date: "2019-07-05"
 */

using UnityEngine;
using komal;
using komal.sdk;
namespace komal.puremvc {
    public class SceneBase : komal.puremvc.ComponentEx 
    {
        protected override void Awake(){
            base.Awake();
            SDKManager.Instance.OnInit();
        }
        
        void OnApplicationPause(bool isPaused) {
            if(isPaused){
                SDKManager.Instance.OnPause();
            }else{
                SDKManager.Instance.OnResume();
            }
        }

        void OnApplicationQuit(){
            SDKManager.Instance.OnDestroy();
        }

        public virtual void Update(){
            SDKManager.Instance.OnUpdate();
        }
    }
}

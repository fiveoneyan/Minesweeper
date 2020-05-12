using UnityEngine;
using komal.sdk;

namespace komal.test {
    public class TestAD : MonoBehaviour 
    {
        public void ShowBanner(){
            komal.sdk.SDKManager.Instance.ShowBanner();
        }

        public void HideBanner(){
            komal.sdk.SDKManager.Instance.HideBanner();
        }

        public void ShowInterstitial(){
            komal.sdk.SDKManager.Instance.ShowInterstitial((InterstitialResult result)=>{
                Debug.Log("Test showInterstitial ResultAD: " + result.ToString());
            });
        }

        public void ShowRewardedVideo(){
            komal.sdk.SDKManager.Instance.ShowRewardedVideo((RewardedVideoResult result)=>{
                Debug.Log("Test showRewardedVideo ResultAD: " + result.ToString());
            });
        }

        public void ValidateIntegration(){
            komal.sdk.SDKManager.Instance.ValidateIntegration();
        }
    }
}

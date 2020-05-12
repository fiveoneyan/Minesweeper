using UnityEngine;

namespace komal.test {
    public class TestGameCenter : MonoBehaviour 
    {
        public void Test_GameCenter_IsAuthenticated(){
            var isAuthenticated = sdk.SDKManager.Instance.IsAuthenticated();
            KomalUtil.Instance.ShowMessage("提示", "用户是否登录：" + isAuthenticated);
        }

        public void Test_GameCenter_Login(){
            sdk.SDKManager.Instance.Login();
        }


        public void Test_GameCenter_OpenGameCenter(){
            sdk.SDKManager.Instance.OpenGameCenter();
        }

        public void Test_GameCenter_RecordRank(){
            sdk.SDKManager.Instance.RecordRank( komal.Config.ID.GetValue("GameTime"), 99, (bool result)=>{
                KomalUtil.Instance.ShowMessage("RecordRank result", result.ToString(), "OK");
            });
        }

        public void Test_GameCenter_RecordAchievement(){
            KomalUtil.Instance.ShowMessage("RecordAchievement result", "未提供测试配置", "OK");
            // sdk.SDKManager.Instance.RecordAchievement(StreamAssetsConfig.IDConfig.HighScore, 1, (bool result)=>{
            //     KomalUtil.Instance.ShowMessage("RecordAchievement result", result.ToString(), "OK");
            // });
        }

        public void Test_GameCenter_FiveStars(){
            sdk.SDKManager.Instance.FiveStars();
        }
    }
}

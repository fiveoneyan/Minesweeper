using UnityEngine;
using System;
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace komal.sdk
{
    public class iOSGameCenter : GameCenterBase, IGameCenter
    {
#region Public API
        // 登录
        public override void Login(Action<bool, string> callback = null){
            Social.localUser.Authenticate((bool success, string info)=>{
                Debug.Log("GameCenter Authenticate >>>> result: " + success + " info: " + info);
                if(callback != null){
                    callback(success, info);
                }
            });
        }

        // 是否 authenticated 
        public override bool IsAuthenticated(){
            return Social.localUser.authenticated;
        }

        public override void OpenGameCenter()
        {
            Social.ShowLeaderboardUI();
        }

        public override void RecordRank(string id, int value, Action<bool> callback = null)
        {
            if (Social.localUser.authenticated){
                Social.ReportScore(value, id, (bool success)=>{
                    if(callback != null){ callback(success); }
                });
            }
        }

        public override void RecordAchievement(string achievementID, double progress, Action<bool> callback = null)
        {
            if (Social.localUser.authenticated){
                Social.ReportProgress(achievementID, progress, (bool success)=>{
                    if(callback != null){ callback(success); }
                });
            }
        }

        public override void FiveStars() {
            #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
            _TAG_UnityiOSGameCenter_FiveStars();
            #endif
        }

        #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _TAG_UnityiOSGameCenter_FiveStars();
        #endif
#endregion
    }
}
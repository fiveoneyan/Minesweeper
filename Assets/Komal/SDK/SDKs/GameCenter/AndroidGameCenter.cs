/* Brief: Android GameCenter
 * Author: Komal
 * Date: "2019-07-17"
 */

using UnityEngine;
using System;

namespace komal.sdk
{
    public class AndroidGameCenter : GameCenterBase, IGameCenter
    {
#region IGameCenter
        public override void Login(Action<bool, string> callback = null)
        {
            m_IsAuthenticated = true;
            if(callback != null){
                callback(true, "Login Success!");
            }
        }

        public override bool IsAuthenticated()
        {
            return m_IsAuthenticated;
        }

        public override void OpenGameCenter()
        {
            Debug.Log("AndroidGameCenter >> 打开游戏中心");
        }

        public override void RecordRank(string key, int value, System.Action<bool> callback = null)
        {
            Debug.Log("AndroidGameCenter >> RecordRank key:" + key + " value:" + value);
            if(callback != null){
                callback(true);
            }
        }

        public override void RecordAchievement(string key, double value, System.Action<bool> callback = null)
        {
            Debug.Log("AndroidGameCenter >> RecordAchievement key:" + key + " value:" + value);
            if(callback != null){
                callback(true);
            }
        }
#endregion

        private bool m_IsAuthenticated = false;
    }
}
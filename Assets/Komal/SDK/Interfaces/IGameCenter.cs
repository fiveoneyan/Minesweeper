using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace komal.sdk
{
    public interface IGameCenter
    {
        // 登录
        void Login(System.Action<bool, string> callback = null);

        // 是否 authenticated 
        bool IsAuthenticated();

        // 打开游戏中心
        void OpenGameCenter();

        // 记录分类排行数据
        void RecordRank(string key, int value, System.Action<bool> callback = null);

        // 记录成就数据
        void RecordAchievement(string key, double value, System.Action<bool> callback = null);

        // 五星好评
        void FiveStars();
    }   
}

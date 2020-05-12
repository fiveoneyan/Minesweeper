/* Brief: GameCenter Base
 * Author: Komal
 * Date: "2019-07-17"
 */

using UnityEngine;
using System;

namespace komal.sdk
{
    public abstract class GameCenterBase : SDKBase, IGameCenter
    {

#region IGameCenter
        public virtual void Login(Action<bool, string> callback = null) { }
        public virtual bool IsAuthenticated() { return false; }
        public virtual void OpenGameCenter() { }
        public virtual void RecordRank(string key, int value, System.Action<bool> callback = null) { }
        public virtual void RecordAchievement(string key, double value, System.Action<bool> callback = null) { }
        public virtual void FiveStars(){ }
#endregion

#region LifeCycle Override
        public override void OnInit(){
            m_isGameTimeEnabled = komal.Config.ID.GetValue("GameTime") != "";
            this.RandomTimeThresdhold();
        }

        public override void OnUpdate(){
            base.OnUpdate();
            // 上传游戏时间到游戏中心
            m_time += Time.deltaTime;
            if(this.IsAuthenticated() && m_time > CONST_TIME){
                var delta = (int)m_time;
                m_time -= delta;
                this.total_time += delta;
                this.RandomTimeThresdhold();
            }
        }
#endregion

#region Upload Game Time
        private double total_time {
            get { return  KomalUtil.Instance.GetItem("game_center_total_time", 0.0); }
            set {
                KomalUtil.Instance.SetItem("game_center_total_time", value);
                if(this.m_isGameTimeEnabled && this.IsAuthenticated()){
                    this.RecordRank(komal.Config.ID.GetValue("GameTime"), (int)value);
                } 
            }
        }
        private void RandomTimeThresdhold(){ CONST_TIME = UnityEngine.Random.Range(30, 61); }
        private bool m_isGameTimeEnabled = false;
        private double m_time = 0;
        private double CONST_TIME = 61.0;
#endregion
    }
}
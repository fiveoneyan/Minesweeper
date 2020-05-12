/* Brief: puremvc 扩展
 * Author: Komal
 * Date: "2019-07-05"
 */
using UnityEngine;
using System;
using System.Collections;

namespace komal.puremvc {
    public class ComponentEx : MonoBehaviour, INotificationHandler, IPoolableEx
    {
        /// <summary>Return the Singleton facade instance</summary>
        protected IFacade facade
        {
            get { return Facade.getInstance(); }
        }

        private static int g_MediatorCount = 0;
        protected int m_ID = 0;
        private KMediator _mediator_ = null;
        protected string mediatorName => _mediator_?.mediatorName;

        protected virtual void Awake(){
            this.CreateMediator();
            m_ID = g_MediatorCount;
        }

        protected virtual void OnDestroy(){
            this.DestoryMediator();
        }

        private void CreateMediator(){
            this.DestoryMediator();
            ++g_MediatorCount;
            this._mediator_ = new KMediator(g_MediatorCount.ToString(), this);
        }

        private void DestoryMediator(){
            if(this._mediator_ != null){
                var mediator = this._mediator_;
                this._mediator_ = null;
                mediator.OnDestroy();
            }
        }

        // can override by sub class
        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public virtual void HandleNotification(INotification notification)
        {
            // can override by sub class
        }

        // delay 秒后，以 duration 的时间间隔执行 callback count 次;
        protected Coroutine StartScheduler(int count, Action<int> callback, float duration = 0.0f,  float delay = 0.0f){
            return StartCoroutine(Scheduler(count, callback, duration, delay));
        }

        private IEnumerator Scheduler(int count, Action<int> callback, float duration = 0.0f, float delay = 0.0f){
            if(delay != 0.0f){
                yield return new WaitForSeconds(delay);
            }
            if(duration == 0.0f){
                for(int i = 0; i<count; i++){
                    callback(i+1);
                    yield return null;
                }
            }else{
                for(int i = 0; i<count; i++){
                    callback(i+1);
                    yield return new WaitForSeconds(duration);
                }
            }
        }

        //////////////////////////////////////////////////////////////
        //// IPoolableEx
        //////////////////////////////////////////////////////////////
        public AllocateState AllocateState { get; set; }
        public virtual void OnPoolableAllocated(bool isReuse) {
            if(isReuse){
                this.CreateMediator();
            }
        }
        public virtual void OnPoolableReleased() {
            this.StopAllCoroutines();
            this.DestoryMediator();
        }

        // PureMVC 模块支持
        internal class KMediator : Mediator
        {
            public KMediator(string mediatorName, object _viewComponent = null) : base(mediatorName, _viewComponent)
            {
                this.facade.RegisterMediator(this);
            }

            public void OnDestroy(){
                this.facade.RemoveMediator(this.mediatorName);
            }

            public override string[] ListNotificationInterests()
            {
                var _viewComponent = this.viewComponent as ComponentEx;
                return _viewComponent.ListNotificationInterests();
            }

            public override void HandleNotification(INotification notification)
            {
                var _viewComponent = this.viewComponent as ComponentEx;
                _viewComponent.HandleNotification(notification);
            }
        }
    }
}


namespace komal.sdk{
    public class SDKBase : komal.puremvc.Mediator, ILifeCycle
    {
        public SDKBase(): base("SDK") { this.facade.RegisterMediator(this); }
        public virtual void OnInit() { }
        public virtual void OnPause() { }
        public virtual void OnResume() { }
        public virtual void OnDestroy() { }
        public virtual void OnUpdate() { }
    }
}

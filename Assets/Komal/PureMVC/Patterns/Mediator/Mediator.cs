namespace komal.puremvc
{
    public class Mediator : Notifier, IMediator, INotifier
    {
        public static string NAME = "Mediator";
        public Mediator(){
            this.mediatorName = Mediator.NAME;
        }
        public Mediator(string _mediatorName, object _viewComponent = null)
        {
            this.mediatorName = _mediatorName ?? Mediator.NAME;
            this.viewComponent = _viewComponent;
        }

        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public virtual void HandleNotification(INotification notification)
        {
        }

        public virtual void OnRegister()
        {
        }

        public virtual void OnRemove()
        {
        }

        public string mediatorName { get; protected set; }

        public object viewComponent { get; set; }
    }
}

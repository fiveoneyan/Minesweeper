namespace komal.puremvc
{
    public class Notifier : INotifier
    {
        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            this.facade.SendNotification(notificationName, body, type);
        }

        /// <summary>Return the Singleton Facade instance</summary>
        protected IFacade facade
        {
            get
            {
                return Facade.getInstance();
            }
        }
    }
}

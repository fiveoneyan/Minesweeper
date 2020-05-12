using System;

namespace komal.puremvc
{
    public class Observer: IObserver
    {
        public Observer(Action<INotification> _notifyMethod, object _notifyContext)
        {
            notifyMethod = _notifyMethod;
            notifyContext = _notifyContext;
        }

        public virtual void NotifyObserver(INotification Notification)
        {
            notifyMethod(Notification);
        }

        public virtual bool CompareNotifyContext(object obj)
        {
            return notifyContext.Equals(obj);
        }

        public Action<INotification> notifyMethod { get; set; }

        public object notifyContext { get; set; }
    }
}

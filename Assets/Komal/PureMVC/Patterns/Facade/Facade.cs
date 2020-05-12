using System;

namespace komal.puremvc
{
    public class Facade : IFacade
    {
        private Facade()
        {
            if (instance != null) throw new Exception(Singleton_MSG);
            instance = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }

        
        public static IFacade getInstance()
        {
            if (instance == null)
            {
                instance = new Facade();
            }
            return instance;
        }

        protected virtual void InitializeController()
        {
            controller = Controller.getInstance(() => new Controller());
        }

        protected virtual void InitializeModel()
        {
            model = Model.getInstance(() => new Model());
        }

        protected virtual void InitializeView()
        {
            view = View.getInstance(() => new View());
        }

        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            controller.RegisterCommand(notificationName, commandFunc);
        }

        public virtual void RemoveCommand(string notificationName)
        {
            controller.RemoveCommand(notificationName);
        }

        public virtual bool HasCommand(string notificationName)
        {
            return controller.HasCommand(notificationName);
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            model.RegisterProxy(proxy);
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return model.RetrieveProxy(proxyName);
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            return model.RemoveProxy(proxyName);
        }

        public virtual bool HasProxy(string proxyName)
        {
            return model.HasProxy(proxyName);
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            view.RegisterMediator(mediator);
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return view.RetrieveMediator(mediatorName);
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return view.RemoveMediator(mediatorName);
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return view.HasMediator(mediatorName);
        }

        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            NotifyObservers(new Notification(notificationName, body, type));
        }

        public virtual void NotifyObservers(INotification notification)
        {
            view.NotifyObservers(notification);
        }

        protected IController controller;
        protected IModel model;
        protected IView view;
        protected static IFacade instance;
        protected const string Singleton_MSG = "Facade Singleton already constructed!";
    }
}

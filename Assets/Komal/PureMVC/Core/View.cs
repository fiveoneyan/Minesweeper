using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace komal.puremvc 
{
    public class View: IView
    {
        public View()
        {
            if (instance != null) throw new Exception(Singleton_MSG);
            instance = this;
            mediatorMap = new ConcurrentDictionary<string, IMediator>();
            observerMap = new ConcurrentDictionary<string, IList<IObserver>>();
            InitializeView();
        }

        protected virtual void InitializeView()
        {
        }

        public static IView getInstance(Func<IView> viewFunc)
        {
            if (instance == null)
            {
                instance = viewFunc();
            }
            return instance;
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if (observerMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                observers.Add(observer);
            }
            else
            {
                observerMap.TryAdd(notificationName, new List<IObserver> { observer });
            }
        }

        public virtual void NotifyObservers(INotification notification)
        {
            // Get a reference to the observers list for this notification name
            if (observerMap.TryGetValue(notification.name, out IList<IObserver> observers_ref))
            {
                // Copy observers from reference array to working array, 
                // since the reference array may change during the notification loop
                var observers = new List<IObserver>(observers_ref);

                // Notify Observers from the working array
                foreach (IObserver observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }

        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            // the observer list for the notification under inspection
            if (observerMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                // find the observer for the notifyContext
                for (int i = 0; i < observers.Count; i++)
                {
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        // there can only be one Observer for a given notifyContext 
					    // in any given Observer list, so remove it and break
                        observers.RemoveAt(i);
                        break;
                    }
                }

                // Also, when a Notification's Observer list length falls to
                // zero, delete the notification key from the observer map
                if (observers.Count == 0)
                    observerMap.TryRemove(notificationName, out IList<IObserver> _);
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            // do not allow re-registration (you must to removeMediator fist)
            // Register the Mediator for retrieval by name
            if(mediatorMap.TryAdd(mediator.mediatorName, mediator))
            {
                // Get Notification interests, if any.
                string[] interests = mediator.ListNotificationInterests();

                // Register Mediator as an observer for each notification of interests
                if (interests.Length > 0)
                {
                    // Create Observer referencing this mediator's handlNotification method
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);

                    // Register Mediator as Observer for its list of Notification interests
                    for (int i = 0; i < interests.Length; i++)
                    {
                        RegisterObserver(interests[i], observer);
                    }
                }
                // alert the mediator that it has been registered
                mediator.OnRegister();
            }
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out IMediator mediator) ? mediator : null;
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            // Retrieve the named mediator
            if (mediatorMap.TryRemove(mediatorName, out IMediator mediator))
            {
                // for every notification this mediator is interested in...
                string[] interests = mediator.ListNotificationInterests();
                for (int i = 0; i < interests.Length; i++)
                {
                    // remove the observer linking the mediator 
					// to the notification interest
                    RemoveObserver(interests[i], mediator);
                }

                // remove the mediator from the map
                mediator.OnRemove();
            }
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }

        protected readonly ConcurrentDictionary<string, IMediator> mediatorMap;
        protected readonly ConcurrentDictionary<string, IList<IObserver>> observerMap;
        protected static IView instance;
        protected const string Singleton_MSG = "View Singleton already constructed!";
    }
}

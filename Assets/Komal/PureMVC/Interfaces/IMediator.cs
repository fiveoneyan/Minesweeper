//
//  PureMVC C# Standard
//
//  Copyright(c) 2017 Saad Shams <saad.shams@puremvc.org>
//  Your reuse is governed by the Creative Commons Attribution 3.0 License
//

namespace komal.puremvc
{
    public interface IMediator: INotifier
    {
        string mediatorName { get; }
        object viewComponent { get; set; }
        string[] ListNotificationInterests();
        void HandleNotification(INotification notification);
        void OnRegister();
        void OnRemove();
    }
}

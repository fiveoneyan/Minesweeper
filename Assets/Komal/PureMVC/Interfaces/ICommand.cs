namespace komal.puremvc 
{
    public interface ICommand: INotifier
    {
        void Execute(INotification Notification);
    }
}

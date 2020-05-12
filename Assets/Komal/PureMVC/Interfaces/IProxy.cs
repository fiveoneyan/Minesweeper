namespace komal.puremvc
{
    public interface IProxy: INotifier
    {
        string proxyName { get; }
        object data { get; set; }
        void OnRegister();
        void OnRemove();
    }
}

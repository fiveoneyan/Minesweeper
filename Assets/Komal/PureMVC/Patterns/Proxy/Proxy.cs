namespace komal.puremvc
{
    public class Proxy: Notifier, IProxy, INotifier
    {
        public static string NAME = "Proxy";
        public Proxy(string _proxyName, object _data = null)
        {
            this.proxyName = _proxyName ?? Proxy.NAME;
            if (_data != null){
                this.data = _data;
            }
        }

        public virtual void OnRegister()
        { 
        }

        public virtual void OnRemove()
        {
        }

        public string proxyName { get; protected set; }
        public object data { get; set; }
    }
}

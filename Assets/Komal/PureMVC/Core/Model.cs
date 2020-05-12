using System;
using System.Collections.Concurrent;

namespace komal.puremvc 
{
    public class Model: IModel
    {
        public Model()
        {
            if (instance != null) throw new Exception(Singleton_MSG);
            instance = this;
            proxyMap = new ConcurrentDictionary<string, IProxy>();
            InitializeModel();
        }
        
        protected virtual void InitializeModel()
        {
        }

        public static IModel getInstance(Func<IModel> modelFunc)
        {
            if (instance == null)
            {
                instance = modelFunc();
            }
            return instance;
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            proxyMap[proxy.proxyName] = proxy;
            proxy.OnRegister();
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return proxyMap.TryGetValue(proxyName, out IProxy proxy) ? proxy : null;
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            if (proxyMap.TryRemove(proxyName, out IProxy proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        public virtual bool HasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }

        protected readonly ConcurrentDictionary<string, IProxy> proxyMap;
        protected static IModel instance;
        protected const string Singleton_MSG = "Model Singleton already constructed!";
    }
}

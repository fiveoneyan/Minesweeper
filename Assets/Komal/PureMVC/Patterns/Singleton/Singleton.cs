/* Brief: ISingleton
 * Author: Komal
 * Date: "2019-07-10"
 */
namespace komal.puremvc {
    interface ISingleton
    {
        void OnSingletonInit();
        string SingletonName();
    }
    public abstract class Singleton<T> : Mediator, ISingleton where T : Singleton<T>, new()  {
        private static T _instance;

        public static T getInstance(){
            if(_instance == null){
                _instance = new T();
                _instance.mediatorName = _instance.SingletonName();
                _instance.facade.RegisterMediator(_instance);
                _instance.OnSingletonInit();
            }
            return _instance;
        }

        public static T Instance
        {
            get {
                return getInstance();
            }
        }

        public abstract void OnSingletonInit();
        public abstract string SingletonName();
    }
}

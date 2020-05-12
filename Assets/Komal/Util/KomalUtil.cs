/* Brief: Komal Util 
 * Author: Komal
 * Date: "2019-07-06"
 */

namespace komal {
    // KomalUtil implemented in partial class
    public partial class KomalUtil: puremvc.Singleton<KomalUtil> {
        // implemented in other files.
        public override void OnSingletonInit(){
            
        }
        public override string SingletonName(){
            return "KomalUtil";
        }
    }

    public static class MSG_LOADING{
        public const string ON = "MSG_LOADING_ON";
        public const string OFF = "MSG_LOADING_OFF";
    }
    
}

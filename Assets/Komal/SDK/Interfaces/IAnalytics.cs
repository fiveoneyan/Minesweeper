/* Brief: Interface for Analytics
 * Author: Komal
 * Date: "2019-07-23"
 */

namespace komal.sdk
{    
    public interface IAnalytics {
        void SendLaunchEvent();
        void SendTerminateEvent();
        void SendBeginePageEvent(string pageName);
        void SendEndPageEvent(string pageName);
        void SendRateEvent(string content_type, string content_id, string rating_value, string max_rating_value);
    }
}

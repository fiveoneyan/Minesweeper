/* Brief: INotificationHandler
 * Author: Komal
 * Date: "2019-07-10"
 */

namespace komal.puremvc {
    interface INotificationHandler
    {
        string[] ListNotificationInterests();
        void HandleNotification(INotification notification);
    }
}

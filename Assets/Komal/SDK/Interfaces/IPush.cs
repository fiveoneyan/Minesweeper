/************************************************************************************
Author: Komal Zheng
Date: "2019-08-19"
Description: IPush
************************************************************************************/

namespace komal.sdk
{
    public interface IPush
    {
        void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay);
    }
}

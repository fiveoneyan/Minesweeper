namespace komal.sdk {
    public interface ISDKNotify
    {
        void NotifyLogin(string _in_data);
        //登出响应
        void NotifyLogout(string _in_data);
        //支付结果响应
        void NotifyPayResult(string _in_data);
        //初始化完毕响应
        void NotifyInitFinish(string _in_data);
        //拓展函数回调响应
        void NotifyExtraFunction(string _json_string);
    }
}

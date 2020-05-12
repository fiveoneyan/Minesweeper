using UnityEngine;

namespace komal.test {
    public class TestDebug : puremvc.ComponentEx
    {
        public void Test_Exception(){
            throw new System.Exception();
        }
        public void Test_OpenOutLog(){
            string message = sdk.SDKManager.Instance.GetRunTimeLogText(); 
            KomalUtil.Instance.ShowMessage("OutLog.txt Last 20 lines", message, "关闭");
        }
    }
}

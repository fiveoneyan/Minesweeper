/* Brief: Test Native MessageBox
 * Author: Komal
 * Date: "2019-07-06"
 */
using System;
using UnityEngine;
using System.Collections.Generic;

namespace komal.test {
    public partial class TestKomalUtil : komal.puremvc.ComponentEx 
    {

#region Native Util
        public void ShowNativeMessage(){
            KomalUtil.Instance.ShowMessage("title", "msgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsgmsg", "ok", (NativeMessageType msgType)=>{
                Debug.Log(">>>>> ShowMessage okCallback");
            });
        }

        public void ShowNativeDialog(){
            KomalUtil.Instance.ShowDialog("title", "message", "yes", "no", (NativeMessageType msgType)=>{
                if(msgType == NativeMessageType.YES){
                    Debug.Log(">>>>> ShowDialog yesCallback");
                }else{
                    Debug.Log(">>>>> ShowDialog noCallback");
                }
            });
        }

        public void ShowRateUs(){
            KomalUtil.Instance.ShowRateUs("title", "message", "rate", "remind", "declined", (NativeMessageType msgType)=>{
                if(msgType == NativeMessageType.RATE){
                    Debug.Log(">>>>> ShowRateUs RATE");
                }else if(msgType == NativeMessageType.REMIND){
                    Debug.Log(">>>>> ShowRateUs REMIND");
                }else if(msgType == NativeMessageType.DECLINED){
                    Debug.Log(">>>>> ShowRateUs DECLINED");
                }
            });
        }

        public void RedirectToAppStoreRatingPage(){
            KomalUtil.Instance.OpenURL(komal.Config.ID.GetValue("AppUrl"));
        }

        public void RedirectToWebPage(){
            KomalUtil.Instance.OpenURL("http://www.baidu.com");
        }
#endregion


#region  IO
        [Serializable]
        private class TestIO_Data {
            public int id;
            public string name;
            public List<TestIO_Item> dic;
            public TestIO_Data(){
                id = 99;
                name = "test_name";
                dic = new List<TestIO_Item>(){
                    new TestIO_Item(2, "item2"),
                    new TestIO_Item(2, "item2")
                };
            }

            public override string ToString(){
                return "id: " + id.ToString();
            }
        }

        [Serializable]
        private class TestIO_Item{
            public int id;
            public string name;
            public TestIO_Item(int _id, string _name){
                id = _id;
                name = _name;
            }
        }
        // 显示所有的相关路径
        public void Test_IO_ShowPathsInfo(){
            KomalUtil.Instance.ShowMessage("Path Info", KomalUtil.Instance.GetPathsInfo());
        }

        // 从 StreamAssets 文件夹下读取 IDConfig.json 文件
        public void Test_IO_ReadFromStreamAssets(){
            // KomalUtil.Instance.GetConfig<T>("path_to_StreamAssets_folder_file", "cache_key");
            KomalUtil.Instance.ShowMessage("提示", "请查看 TestKomalUtil.cs 文件中的注释部分");
        }

        public void Test_IO_IsFileExistInPersistentDataPath(){
            string testFilePath = "test_1.json";
            bool isExist = KomalUtil.Instance.IsFileExistInPersistentDataPath(testFilePath);
            string title = "TestIsFileExistInPersistentDataPath";
            string message = string.Format("File: {0} isExsits {1}", testFilePath, isExist.ToString());
            if(isExist){
                KomalUtil.Instance.ShowDialog(title, message, "删除", "取消", (NativeMessageType msgType)=>{
                    if(msgType == NativeMessageType.YES){
                        KomalUtil.Instance.RemoveFile(testFilePath);
                    }
                });
            }else{
                KomalUtil.Instance.ShowMessage(title, message);
            }
        }

        public void Test_IO_RemoveFile(){
            string testFilePath = "test_1.json";
            KomalUtil.Instance.RemoveFile(testFilePath);
            KomalUtil.Instance.ShowMessage("提示", string.Format("删除文件 {0} 成功", testFilePath));
        }

        public void Test_IO_ReadFromPersistentData(){
            string testFilePath = "test_1.json";
            bool isExist = KomalUtil.Instance.IsFileExistInPersistentDataPath(testFilePath);
            if(isExist){
                var data = KomalUtil.Instance.ReadFromPersistentData<TestIO_Data>(testFilePath);
                KomalUtil.Instance.ShowMessage("File: " + testFilePath, data.ToString());
            }else{
                KomalUtil.Instance.ShowMessage("Info", string.Format("File: {0} isExsits {1}", testFilePath, isExist.ToString()));
            }
        }

        public void Test_IO_WriteToPersistentData(){
            string testFilePath = "test_1.json";
            var data = new TestIO_Data();
            data.id = 99;
            KomalUtil.Instance.WriteToPersistentData(testFilePath, data);
            this.Test_IO_IsFileExistInPersistentDataPath();
        }
#endregion

#region Vibrate
        public void Test_Vibrate_NORMAL(){
            KomalUtil.Instance.Vibrate(VibrateType.NORMAL);
        }
        public void Test_Vibrate_PEEK(){
            KomalUtil.Instance.Vibrate(VibrateType.PEEK);
        }
        public void Test_Vibrate_POP(){
            KomalUtil.Instance.Vibrate(VibrateType.POP);
        }
        public void Test_Vibrate_CONTINUE(){
            KomalUtil.Instance.Vibrate(VibrateType.CONTINUE);
        }
#endregion


#region LocalStorage
        private class TestLocalStorage_Data {
            public int id = 0;

            public override string ToString(){
                return "id: " + id.ToString();
            }
        }

        public void Test_LocalStorage_GetItem_string(){
            var ret = KomalUtil.Instance.GetItem("test_string", "This is a default string");
            KomalUtil.Instance.ShowMessage("test_string title", ret);
        }

        public void Test_LocalStorage_SetItem_string(){
            KomalUtil.Instance.SetItem("test_string", "This is a Storaged string");
        }

        public void Test_LocalStorage_GetItem_Object(){
            var ret = KomalUtil.Instance.GetItem("test_obj", new TestLocalStorage_Data());
            KomalUtil.Instance.ShowMessage("test_obj title", ret.ToString());
        }

        public void Test_LocalStorage_SetItem_Object(){
            var obj = new TestLocalStorage_Data();
            obj.id = 100;
            KomalUtil.Instance.SetItem("test_obj", obj);
        }
#endregion

#region Network
        public void Test_Network_IsNetworkReachability(){
            var ret = KomalUtil.Instance.IsNetworkReachability();
            KomalUtil.Instance.ShowMessage("Network Info", string.Format("IsNetworkReachability: {0} Network Type: {1}", ret, Application.internetReachability));
        }
#endregion
    }
}

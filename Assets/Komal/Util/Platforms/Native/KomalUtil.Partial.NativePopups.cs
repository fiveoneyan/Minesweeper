/* Brief: Native Util for KomalUtil
 * Author: Komal
 * Date: "2019-07-08"
 */

using UnityEngine;
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace komal {
    public enum NativeMessageType {
        OK,
        YES,
        NO,
        RATE,
        REMIND,
        DECLINED
    }
    // implementation for native contect
    interface INativePopups {
        void ShowMessage(string title, string message, string ok = "OK", System.Action<NativeMessageType> callback = null);
        void ShowDialog(string title, string message, string yes, string no, System.Action<NativeMessageType> callback = null);
        void ShowRateUs(string title, string message, string rate, string remind, string declined, System.Action<NativeMessageType> callback = null);
    }
    
    public partial class KomalUtil: INativePopups
    {
        public void ShowMessage(string title, string message, string ok = "OK", System.Action<NativeMessageType> callback = null){
            new NativeMessage(title, message, ok, callback);
        }

        public void ShowDialog(string title, string message, string yes, string no, System.Action<NativeMessageType> callback = null){
            new NativeDialog(title, message, yes, no, callback);
        }

        public void ShowRateUs(string title, string message, string rate, string remind, string declined, System.Action<NativeMessageType> callback){
            new NativeRateUs(title, message, rate, remind, declined, callback);
        }

        private class NativeMessage
        {
            public NativeMessage(string title, string message, string ok = "OK", System.Action<NativeMessageType> callback = null)
            {
            #if UNITY_EDITOR || UNITY_ANDROID || UNITY_STANDALONE_OSX
                NativeUnityEditorMessage.Create(title, message, ok, callback);
            #elif (UNITY_IPHONE || UNITY_IOS)
                NativeiOSMessage.Create(title, message, ok, callback);
            #endif
            }

            // Unity 
            private class NativeUnityEditorMessage : UnityEditorAlertBase{
                public string title;
                public string message;
                public string ok;
                public System.Action<NativeMessageType> callback;

                public static NativeUnityEditorMessage Create(string title, string message, string ok = "OK", System.Action<NativeMessageType> callback = null)
                {
                    NativeUnityEditorMessage _ui = new GameObject("NativeUnityEditorMessage").AddComponent<NativeUnityEditorMessage>();
                    _ui.title = title;
                    _ui.message = message;
                    _ui.ok = ok;
                    _ui.callback = callback;
                    
                    _ui.Show();
                    return _ui;
                }

                public void Show()
                {
                    var _canvas = this.canvas;
                    if(_canvas != null){
                        GameObject messagePrefab = Resources.Load("UnityEditorPopupsMessage") as GameObject;
                        var obj = Instantiate(messagePrefab);
                        obj.transform.SetParent(_canvas.transform, false);
                        var comp = obj.GetComponent<UnityEditorPopupsMessage>();
                        comp.Init(title, message, ok, OnNativeCallback);
                    }
                }
                

                // Native Callback
                public void OnNativeCallback(string buttonIndex)
                {
                    if(this.callback != null){
                        this.callback(NativeMessageType.OK);
                    }
                    Destroy(gameObject);
                }
            }

            // iOS
            private class NativeiOSMessage : MonoBehaviour 
            {
                public string title;
                public string message;
                public string ok;
                public System.Action<NativeMessageType> callback;

                public static NativeiOSMessage Create(string title, string message, string ok = "OK", System.Action<NativeMessageType> callback = null)
                {
                    NativeiOSMessage _ui = new GameObject("NativeiOSMessage").AddComponent<NativeiOSMessage>();
                    _ui.title = title;
                    _ui.message = message;
                    _ui.ok = ok;
                    _ui.callback = callback;
                    
                    _ui.Show();
                    return _ui;
                }

                public void Show()
                {
                    iOSNative.ShowMessage(title, message, ok);
                }

                // Native Callback
                public void OnNativeCallback(string buttonIndex)
                {
                    if(this.callback != null){
                        this.callback(NativeMessageType.OK);
                    }
                    Destroy(gameObject);
                }
            }
        }

        
        private class NativeDialog
        {

            public NativeDialog(string title, string message, string yes = "Yes", string no = "No", System.Action<NativeMessageType> callback = null)
            {
            #if UNITY_EDITOR || UNITY_ANDROID || UNITY_STANDALONE_OSX
                NativeUnityEditorDialog.Create(title, message, yes, no, callback);
            #elif (UNITY_IPHONE || UNITY_IOS)
                NativeiOSDialog.Create(title, message, yes, no, callback);
            #endif
            }

            // Unity 
            private class NativeUnityEditorDialog : UnityEditorAlertBase{
                public string title;
                public string message;
                public string yes;
                public string no;
                public System.Action<NativeMessageType> callback;

                public static NativeUnityEditorDialog Create(string title, string message, string yes, string no, System.Action<NativeMessageType> callback = null){
                    NativeUnityEditorDialog _ui = new GameObject("NativeUnityEditorDialog").AddComponent<NativeUnityEditorDialog>();
                    _ui.title = title;
                    _ui.message = message;
                    _ui.yes = yes;
                    _ui.no = no;
                    _ui.callback = callback;
                    _ui.Show();
                    return _ui;
                }

                public void Show()
                {
                    var _canvas = this.canvas;
                    if(_canvas != null){
                        GameObject messagePrefab = Resources.Load("UnityEditorPopupsDialog") as GameObject;
                        var obj = Instantiate(messagePrefab);
                        obj.transform.SetParent(_canvas.transform, false);
                        var comp = obj.GetComponent<UnityEditorPopupsDialog>();
                        comp.Init(title, message, yes, no, OnNativeCallback);
                    }
                }

                // Native Callback
                public void OnNativeCallback(string buttonIndex)
                {
                    if(this.callback!=null){
                        int index = System.Convert.ToInt16(buttonIndex);
                        switch (index)
                        {
                            case 0: 
                                this.callback(NativeMessageType.YES);
                                break;
                            case 1: 
                                this.callback(NativeMessageType.NO);
                            break;
                        }
                    }
                    Destroy(this.gameObject);
                }                
            }

            // iOS
            private class NativeiOSDialog : MonoBehaviour
            {
                public string title;
                public string message;
                public string yes;
                public string no;
                public System.Action<NativeMessageType> callback;

                public static NativeiOSDialog Create(string title, string message, string yes, string no, System.Action<NativeMessageType> callback = null){
                    NativeiOSDialog _ui = new GameObject("NativeiOSDialog").AddComponent<NativeiOSDialog>();
                    _ui.title = title;
                    _ui.message = message;
                    _ui.yes = yes;
                    _ui.no = no;
                    _ui.callback = callback;
                    _ui.Show();
                    return _ui;
                }

                public void Show(){
                    iOSNative.ShowDialog(title, message, yes, no);
                }

                // Native Callback
                public void OnNativeCallback(string buttonIndex)
                {
                    if(this.callback!=null){
                        int index = System.Convert.ToInt16(buttonIndex);
                        switch (index)
                        {
                            case 0: 
                                this.callback(NativeMessageType.YES);
                                break;
                            case 1: 
                                this.callback(NativeMessageType.NO);
                            break;
                        }
                    }
                    Destroy(this.gameObject);
                }
            }
        }


        private class NativeRateUs
        {
            public NativeRateUs(string title, string message, string rate, string remind, string declined, System.Action<NativeMessageType> callback)
            {
            #if UNITY_EDITOR || UNITY_ANDROID
                NativeUnityEditorRateUs.Create(title, message, rate, remind, declined, callback);
            #elif (UNITY_IPHONE || UNITY_IOS)
                NativeiOSRateUs.Create(title, message, rate, remind, declined, callback);
            #endif
            }

            private class NativeUnityEditorRateUs : UnityEditorAlertBase
            {
                public string title;
                public string message;
                public string rate;
                public string remind;
                public string declined;
                public System.Action<NativeMessageType> callback;

                public static NativeUnityEditorRateUs Create(string title, string message, string rate, string remind, string declined, System.Action<NativeMessageType> callback){
                    NativeUnityEditorRateUs _ui = new GameObject("NativeUnityEditorRateUs").AddComponent<NativeUnityEditorRateUs>();
                    _ui.title = title;
                    _ui.message = message;
                    _ui.rate = rate;
                    _ui.remind = remind;
                    _ui.declined = declined;
                    _ui.callback = callback;
                    _ui.Show();
                    return _ui;
                }

                public void Show(){
                    var _canvas = this.canvas;
                    if(_canvas != null){
                        GameObject messagePrefab = Resources.Load("UnityEditorPopupsRateUS") as GameObject;
                        var obj = Instantiate(messagePrefab);
                        obj.transform.SetParent(_canvas.transform, false);
                        var comp = obj.GetComponent<UnityEditorPopupsRateUS>();
                        comp.Init(title, message, rate, remind, declined, OnNativeCallback);
                    }
                }

                // Native Callback
                public void OnNativeCallback(string buttonIndex)
                {
                    if(this.callback!=null){
                        int index = System.Convert.ToInt16(buttonIndex);
                        switch (index)
                        {
                            case 0: 
                                this.callback(NativeMessageType.RATE);
                                break;
                            case 1: 
                                this.callback(NativeMessageType.REMIND);
                                break;
                            case 2: 
                                this.callback(NativeMessageType.DECLINED);
                                break;
                        }
                    }
                    Destroy(this.gameObject);
                }
            }

            private class NativeiOSRateUs : MonoBehaviour
            {
                public string title;
                public string message;
                public string rate;
                public string remind;
                public string declined;
                public System.Action<NativeMessageType> callback;

                public static NativeiOSRateUs Create(string title, string message, string rate, string remind, string declined, System.Action<NativeMessageType> callback){
                    NativeiOSRateUs _ui = new GameObject("NativeiOSRateUs").AddComponent<NativeiOSRateUs>();
                    _ui.title = title;
                    _ui.message = message;
                    _ui.rate = rate;
                    _ui.remind = remind;
                    _ui.declined = declined;
                    _ui.callback = callback;
                    _ui.Show();
                    return _ui;
                }

                public void Show(){
                    iOSNative.ShowRateUs(title, message, rate, remind, declined);
                }

                // Native Callback
                public void OnNativeCallback(string buttonIndex)
                {
                    if(this.callback!=null){
                        int index = System.Convert.ToInt16(buttonIndex);
                        switch (index)
                        {
                            case 0: 
                                this.callback(NativeMessageType.RATE);
                                break;
                            case 1: 
                                this.callback(NativeMessageType.REMIND);
                                break;
                            case 2: 
                                this.callback(NativeMessageType.DECLINED);
                                break;
                        }
                    }
                    Destroy(this.gameObject);
                }
            }
        }

        private class UnityEditorAlertBase: MonoBehaviour{
            protected GameObject canvas{
                get {
                    try{
                        var _ret = FindObjectOfType<Canvas>();
                        if ( _ret!= null)
                        {
                            return _ret.gameObject;
                        }
                        else
                        {
                            return GameObject.Find("Canvas").gameObject;;
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log("获取 canvas 节点失败原因：" + e);
                    }
                    return null;
                }
            }
        }

        private class iOSNative
        {

        #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
            [DllImport("__Internal")]
            private static extern void _TAG_iOSNativePopups_ShowRateUs(string title, string message, string rate, string remind, string declined);

            [DllImport("__Internal")]
            private static extern void _TAG_iOSNativePopups_ShowDialog(string title, string message, string yes, string no);

            [DllImport("__Internal")]
            private static extern void _TAG_iOSNativePopups_ShowMessage(string title, string message, string ok);

            [DllImport("__Internal")]
            private static extern void _TAG_iOSNativePopups_DismissCurrentAlert();
        #endif
            
            public static void ShowRateUs(string title, string message, string rate, string remind, string declined)
            {
            #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_iOSNativePopups_ShowRateUs(title, message, rate, remind, declined);
            #endif
            }

            public static void ShowDialog(string title, string message, string yes, string no)
            {
            #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_iOSNativePopups_ShowDialog(title, message, yes, no);
            #endif
            }

            public static void ShowMessage(string title, string message, string ok)
            {
            #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_iOSNativePopups_ShowMessage(title, message, ok);
            #endif
            }

            public static void DismissCurrentAlert()
            {
            #if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                _TAG_iOSNativePopups_DismissCurrentAlert();
            #endif
            }
        }
    }
}

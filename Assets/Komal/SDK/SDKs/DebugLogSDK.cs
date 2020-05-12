/* Brief: Debug Log SDK, hook and log UnityEngine Debug Log System.
 * Author: Komal
 * Date: "2019-07-11"
 */
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace komal.sdk {
    public class DebugLogSDK : SDKBase, IDebugLog
    {
        private static readonly string m_sLogFile = "OutLog.txt"; 
        private static List<string> mLines = new List<string>();
	    private static List<string> mWriteTxt = new List<string>();
        private StreamWriter m_Writer = null;
        public string GetLogFileFullPath() {
            return KomalUtil.Instance.GetFilePathOfPersistentDataPath(m_sLogFile);
        }

        public string GetRunTimeLogText(){
            return mLines.Aggregate("", (string acc, string ele)=>string.Format("{0}||{1}", acc, ele));
        }
#region ILifeCycle overrides
        public override void OnInit() {
            // 每次启动客户端删除之前保存的 Log
            if(KomalUtil.Instance.IsFileExistInPersistentDataPath(m_sLogFile)){
                KomalUtil.Instance.RemoveFile(m_sLogFile);
            }
            m_Writer = new StreamWriter( GetLogFileFullPath(), true, System.Text.Encoding.UTF8);
            // log hook
            Application.logMessageReceived += LogHandler;
        }

        public override void OnDestroy(){
            m_Writer.Close();
        }

        public override void OnUpdate() {
            base.OnUpdate();
            // 写入操作必须在主线程中完成
            if(mWriteTxt.Count > 0)
            {
                string[] temp = mWriteTxt.ToArray();
                foreach(string t in temp)
                {
                    m_Writer.WriteLine(t);
                    mWriteTxt.Remove(t);
                }
            }
        }
#endregion


        private void LogHandler(string condition, string stackTrace, LogType type){
            mWriteTxt.Add(condition);
            if (type == LogType.Error || type == LogType.Exception) 
            {
                Log(condition);
                Log(stackTrace);
            }
        }

        //这里我把错误的信息保存起来，用来输出在手机屏幕上
        static public void Log (params object[] objs)
        {
            string text = "";
            for (int i = 0; i < objs.Length; ++i)
            {
                if (i == 0)
                {
                    text += objs[i].ToString();
                }
                else
                {
                    text += ", " + objs[i].ToString();
                }
            }
            if (Application.isPlaying)
            {
                if (mLines.Count > 20) 
                {
                    mLines.RemoveAt(0);
                }
                mLines.Add(text);
            }
        }
    }
}

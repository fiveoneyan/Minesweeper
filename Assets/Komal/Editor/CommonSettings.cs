#if UNITY_IPHONE || UNITY_IOS 
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace komal.Editor
{
	public class CommonSettings : ISDKSettings
	{
		public void updateProject (BuildTarget buildTarget, string buildPath, string projectPath, string plistPath)
		{
#region project settings
            /* 第三方 SDK 包含定位功能，增加以下配置
                info.plist 中配置 NSLocationWhenInUseUsageDescription 申请定位权限
                Privacy - Location When In Use Usage Description
                ${DISPLAY_NAME} requires user's location for better user experience.
            */ 
			PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            PlistElementDict root = plist.root;
            PlistElementString eleString = new PlistElementString("${DISPLAY_NAME} requires user's location for better user experience.");
            root["NSLocationWhenInUseUsageDescription"] = eleString;
            plist.WriteToFile(plistPath);


			PBXProject project = new PBXProject ();
			project.ReadFromString (File.ReadAllText (projectPath));
			string targetId = project.TargetGuidByName (PBXProject.GetUnityTargetName ());
			// Required System Frameworks
			project.RemoveFrameworkFromProject (targetId, "LocalAuthentication.framework");
			File.WriteAllText (projectPath, project.WriteToString ());
#endregion

#region plist settings
            // do nothing
#endregion
		}
	}
}
#endif

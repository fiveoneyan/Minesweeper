/************************************************************************************
Author: Komal Zheng
Date: "2019-08-08"
Description: AppsFlyer SDK Settings
************************************************************************************/


#if UNITY_IPHONE || UNITY_IOS 
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System.IO;

namespace komal.Editor
{
	public class AppsFlyerSettings : ISDKSettings
	{
		public void updateProject (BuildTarget buildTarget, string buildPath, string projectPath, string plistPath)
		{
            PBXProject project = new PBXProject ();
			project.ReadFromString (File.ReadAllText (projectPath));

			string targetId = project.TargetGuidByName (PBXProject.GetUnityTargetName ());

			// Required System Frameworks
			project.AddFrameworkToProject (targetId, "Security.framework", false);

            /*
                只有当您包含此框架时，AppsFlyer 才会收集 IDFA。
                不添加此框架就无法追踪 Facebook、Twitter 以及大多数其他广告平台。
             */
			project.AddFrameworkToProject (targetId, "AdSupport.framework", false);

            /*
                强烈建议您将此框架添加到您的应用项目中，因为该框架是追踪 Apple Search Ads 的必备条件。
             */
			project.AddFrameworkToProject (targetId, "iAd.framework", false);

			File.WriteAllText (projectPath, project.WriteToString ());
		}
	}
}
#endif

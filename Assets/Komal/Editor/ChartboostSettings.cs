/* Brief: Chartboost settings
 * Author: Komal
 * Date: "2019-07-12"
 */


#if UNITY_IPHONE || UNITY_IOS 
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System.IO;

namespace komal.Editor
{
	public class ChartboostSettings : ISDKSettings
	{
		public void updateProject (BuildTarget buildTarget, string buildPath, string projectPath, string plistPath)
		{
#region plist settings
            Debug.Log ("Komal - Update project for Chartboost");

			PBXProject project = new PBXProject ();
			project.ReadFromString (File.ReadAllText (projectPath));

			string targetId = project.TargetGuidByName (PBXProject.GetUnityTargetName ());

			// Required System Frameworks
			project.AddFrameworkToProject (targetId, "CoreGraphics.framework", false);
			project.AddFrameworkToProject (targetId, "UIKit.framework", false);

			File.WriteAllText (projectPath, project.WriteToString ());
#endregion
		}
	}
}
#endif

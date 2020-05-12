#if UNITY_IOS 

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace komal.Editor
{
	public class KomalBuildPostprocessor
	{
		[PostProcessBuild(100)]
		public static void OnPostprocessBuild (BuildTarget buildTarget, string buildPath)
		{
			if (buildTarget == BuildTarget.iOS) {
				string projectPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
				string dirpath = Application.dataPath + "/Komal/Editor/";
				string currentNamespace = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
                string plistPath = buildPath + "/Info.plist";

				if (Directory.Exists (dirpath)) {
					//Match the classes that has "Settings" in their name, and don't start with "I"
					var files = Directory.GetFiles (dirpath, "*.cs", SearchOption.TopDirectoryOnly).Where (file => Regex.IsMatch (Path.GetFileName (file), "^(?!I).+Settings.*$"));

					//Go over all the adapter settings classes, and call their updateProject method
					foreach (string file in files) {
						string classname = Path.GetFileNameWithoutExtension (file);

						if (!String.IsNullOrEmpty (classname)) {
							ISDKSettings adapter = (ISDKSettings)Activator.CreateInstance (Type.GetType (currentNamespace + "." + classname));
							adapter.updateProject (buildTarget, buildPath, projectPath, plistPath);
						}
					}
				}
			}

			Debug.Log("Komal build postprocessor finished");
		}
	}
}
#endif

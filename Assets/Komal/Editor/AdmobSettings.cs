/* Brief: Admob settings
 * Author: Komal
 * Date: "2019-07-10"
 * intergrate admob into iOS 
 * https://developers.ironsrc.com/ironsource-mobile/unity/admob-mediation-guide/#step-4
 */

#if UNITY_IPHONE || UNITY_IOS 
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace komal.Editor
{
	public class AdmobSettings : ISDKSettings
	{
		public void updateProject (BuildTarget buildTarget, string buildPath, string projectPath, string plistPath)
		{
#region plist settings
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            PlistElementDict root = plist.root;
            PlistElementString eleString = new PlistElementString(komal.Config.ID.GetValue("AdMobAppId"));
            root["GADApplicationIdentifier"] = eleString;
            plist.WriteToFile(plistPath);
#endregion
		}
	}
}
#endif

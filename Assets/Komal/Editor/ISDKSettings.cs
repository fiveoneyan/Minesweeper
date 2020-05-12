
using UnityEditor;

namespace komal.Editor
{
	public interface ISDKSettings
	{
		void updateProject (BuildTarget buildTarget, string buildPath, string projectPath, string plistPath);
	}
}

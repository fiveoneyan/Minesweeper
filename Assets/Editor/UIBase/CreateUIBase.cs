using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

public class CreateUIBase : Editor
{

    [MenuItem("Assets/Create/UIBase", false, 82)]
    public static void CreatNewUICSharp()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
        ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(),
        GetSelectedPathOrFallback() + "/NewUIScript.cs",
        null,
       Application.dataPath + "/Editor/UIBase/UIBase.cs.txt");
    }



    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}


class MyDoCreateScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        UnityEngine.Object o = CreateScriptAssetFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }

    internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFile);
        string text = streamReader.ReadToEnd();
        streamReader.Close();

        //TODO:脚本内容处理
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
        text = Regex.Replace(text, "#DATE#", System.DateTime.Now.ToString("yyyy-MM-dd"));

        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }
}

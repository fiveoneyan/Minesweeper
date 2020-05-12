using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
using System.Text;
 
public class CreateLuaAuto {
    // 菜单路径，是否为验证方法，菜单项优先级; 方法必须为静态方法 
    [MenuItem("Assets/Create/Komal C# Class:ComponentEx", false, 70)]
    public static void CreateComponentEx()
    {
        //参数为传递给 CreateTemplateScriptAssets 类action方法的参数
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<CreateTemplateScriptAssets>(),
            GetSelectPathOrFallback() + "/NewComponent.cs", null,
            "Assets/Komal/Templates/NewComponent.cs.txt");
    }
    //取得要创建文件的路径
    public static string GetSelectPathOrFallback()
    {
        string path = "Assets";
        //遍历选中的资源以获得路径
        //Selection.GetFiltered是过滤选择文件或文件夹下的物体，assets表示只返回选择对象本身
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
 
//要创建模板文件必须继承EndNameEditAction，重写action方法
class CreateTemplateScriptAssets : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            //创建资源
            UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源
        }
 
        internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
        {
            //获取要创建资源的绝对路径
            string fullPath = Path.GetFullPath(pathName);
            //读取本地的模板文件
            StreamReader streamReader = new StreamReader(resourceFile);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            //获取文件名，不含扩展名
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
 
            //将模板类中的类名替换成你创建的文件名
            text = Regex.Replace(text, "NewClass", fileNameWithoutExtension);
            bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
            bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
            bool append = false;
            //写入文件
            StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
            streamWriter.Write(text);
            streamWriter.Close();
            //刷新资源管理器
            AssetDatabase.ImportAsset(pathName);
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
        }
}

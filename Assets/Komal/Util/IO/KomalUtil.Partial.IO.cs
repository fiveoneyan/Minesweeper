/* Brief: IO Util
 * Author: Komal
 * Date: "2019-07-10"
 */
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace komal {
    public partial class KomalUtil 
    {
#region Common
        public string GetPathsInfo(){
            return string.Format(@"streamingAssetsPath: {0} persistentDataPath: {1} dataPath: {2} ", streamingAssetsPath, persistentDataPath, UnityEngine.Application.dataPath);
        }
#endregion

#region StreamingAssets
        private string streamingAssetsPath {
            get {
                return UnityEngine.Application.streamingAssetsPath;
            }
        }

        private string GetFilePathOfStreamingAssetsPath(string filePath){
            return streamingAssetsPath + Path.DirectorySeparatorChar + filePath;
        }

        private string __ReadFromStreamAssets(string jsonFilePath){
            var fullPath = GetFilePathOfStreamingAssetsPath(jsonFilePath);
            #if UNITY_ANDROID && !UNITY_EDITOR
                WWW www= new WWW(fullPath);
                if (www.error != null)
                {
                    Debug.LogError("error : " + fullPath);
                    return "";
                }
                while (!www.isDone) {}
                return www.text;
            #else
                StreamReader reader = new StreamReader( fullPath );
                if (reader != null)
                {
                    var ret = reader.ReadToEnd();
                    reader.Close();
                    return ret;
                }else{
                    throw new System.Exception();
                }
            #endif
        }

        public T ReadFromStreamAssets<T>(string jsonFilePath)
        {
            return JsonUtility.FromJson<T>( __ReadFromStreamAssets(jsonFilePath) );
        }
#endregion


#region PersistentData
        private string persistentDataPath {
            get {
                return UnityEngine.Application.persistentDataPath;
            }
        }

        public string GetFilePathOfPersistentDataPath(string filePath){
            return persistentDataPath + Path.DirectorySeparatorChar + filePath;
        }

        public bool IsFileExistInPersistentDataPath(string filePath){
            return System.IO.File.Exists( GetFilePathOfPersistentDataPath(filePath) );
        }

        public void RemoveFile(string jsonFilePath){
            if(IsFileExistInPersistentDataPath(jsonFilePath)){
                FileInfo fileInfo = new FileInfo( GetFilePathOfPersistentDataPath(jsonFilePath) );
                if(fileInfo != null){
                    fileInfo.Delete();
                }
            }else{
                Debug.Log(string.Format("File {0} is not Exist in {1}!", jsonFilePath, persistentDataPath));
            }
            // Editor Mode TODO
            // UnityEditor.FileUtil.DeleteFileOrDirectory("yourPath/YourFileOrFolder");
        }

        public T ReadFromPersistentData<T>(string jsonFilePath){
            StreamReader reader = new StreamReader( GetFilePathOfPersistentDataPath(jsonFilePath) );
            if (reader != null)
            {
                var enCryptString = reader.ReadToEnd();
                #if UNITY_EDITOR || UNITY_STANDALONE_OSX
                string jsonString = enCryptString;
                #else
                string jsonString = Crypto.Decrypt(enCryptString);
                #endif
                var ret = UnityEngine.JsonUtility.FromJson<T>(jsonString);
                reader.Close();
                return ret;
            }else{
                throw new System.ArgumentException();
            }
        }

        public void WriteToPersistentData<T>(string jsonFilePath, T data, bool append = false){
            var path = GetFilePathOfPersistentDataPath(jsonFilePath);
            var mode = IsFileExistInPersistentDataPath(path) ? FileMode.Truncate : FileMode.Create;
            var fs = new FileStream( path, mode, FileAccess.Write, FileShare.ReadWrite );
            var writer = new StreamWriter( fs, Encoding.UTF8);
            if (writer != null)
            {
                string jsonString = UnityEngine.JsonUtility.ToJson(data);
                #if UNITY_EDITOR || UNITY_STANDALONE_OSX
                string enCryptString = jsonString;
                #else
                var enCryptString = Crypto.Encrypt(jsonString);
                #endif
                writer.Write(enCryptString);
                writer.Close();
                fs.Close();
            }else{
                throw new System.ArgumentException();
            }
        }
#endregion

#region EnCryption
        private class Crypto {
            private static byte[]  m_Key = UTF8Encoding.UTF8.GetBytes("88123456789012345678901234567890");

            public static string Encrypt(string toEncrypt){
                var cTransform = CreateRijndaelManaged().CreateEncryptor();
                var toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            public static string Decrypt(string toDecrypt){
                var cTransform = CreateRijndaelManaged().CreateDecryptor();
                var toDecryptArray = Convert.FromBase64String(toDecrypt);
                var resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return UTF8Encoding.UTF8.GetString(resultArray);
            }

            private static RijndaelManaged CreateRijndaelManaged(){
                var rDel = new RijndaelManaged();
                rDel.Key = m_Key;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;
                return rDel;
            }
        }
#endregion
    }
}

/* Brief: LocalStorage 用户数据记录模块
 * Author: Komal
 * Date: "2019-07-15"
 */
using System.Collections.Generic;

namespace komal {
    public partial class KomalUtil {
        // 基础数据类型
        [System.Serializable]
        private class LocalStorageTypeData<T> {
            public T data;
        }

        // 读写类
        private class LocalStorage {
            private static string JsonFilePath(string localStorageKey){
                return "ls_" + localStorageKey + ".json";
            }
            private static Dictionary<string, object> m_Cache = new Dictionary<string, object>();

            public static T GetItem<T>(string localStorageKey, T defaultValue, bool isObject){
                object _data;
                if (!m_Cache.TryGetValue(localStorageKey, out _data)) {
                    // the key isn't in the dictionary.
                    _data = (object)ReadItem<T>(localStorageKey, defaultValue, isObject);
                    m_Cache.Add(localStorageKey, _data);
                }
                return (T)_data;
            }

            public static void SetItem<T>(string localStorageKey, T value, bool isObject){
                object _data;
                if (!m_Cache.TryGetValue(localStorageKey, out _data)) {
                    WriteItem<T>(localStorageKey, value, isObject);
                }else{
                    m_Cache[localStorageKey] = value;
                    WriteItem<T>(localStorageKey, value, isObject);
                } 
            }

            private static T ReadItem<T>(string localStorageKey, T defaultValue, bool isObject){
                var jsonFilePath = JsonFilePath(localStorageKey);
                if(Instance.IsFileExistInPersistentDataPath(jsonFilePath)){
                    if(isObject){
                        return KomalUtil.Instance.ReadFromPersistentData<T>( jsonFilePath );
                    }else{
                        var typeData = KomalUtil.Instance.ReadFromPersistentData<LocalStorageTypeData<T>>( jsonFilePath );
                        return typeData.data;
                    }
                }else{
                    WriteItem<T>(localStorageKey, defaultValue, isObject);
                    return defaultValue;
                }
            }

            private static void WriteItem<T>(string localStorageKey, T value, bool isObject){
                if(isObject){
                    KomalUtil.Instance.WriteToPersistentData( JsonFilePath(localStorageKey), value );
                }else{
                    var wrapValue = new LocalStorageTypeData<T>();
                    wrapValue.data = value;
                    KomalUtil.Instance.WriteToPersistentData( JsonFilePath(localStorageKey), wrapValue );
                }
            }
        }

        public T GetItem<T>(string localStorageKey, T defaultValue){
            return LocalStorage.GetItem(localStorageKey, defaultValue, true);
        }

        public void SetItem<T>(string localStorageKey, T value){
            LocalStorage.SetItem(localStorageKey, value, true);
        }

        public string GetItem(string localStorageKey, string defaultValue){
            return LocalStorage.GetItem<string>(localStorageKey, defaultValue, false);
        }

        public void SetItem(string localStorageKey, string value){
            LocalStorage.SetItem(localStorageKey, value, false);
        }

        public int GetItem(string localStorageKey, int defaultValue){
            return LocalStorage.GetItem<int>(localStorageKey, defaultValue, false);
        }

        public void SetItem(string localStorageKey, int value){
            LocalStorage.SetItem(localStorageKey, value, false);
        }

        public double GetItem(string localStorageKey, double defaultValue){
            return LocalStorage.GetItem<double>(localStorageKey, defaultValue, false);
        }

        public void SetItem(string localStorageKey, double value){
            LocalStorage.SetItem(localStorageKey, value, false);
        }

        public bool GetItem(string localStorageKey, bool defaultValue){
            return LocalStorage.GetItem<bool>(localStorageKey, defaultValue, false);
        }

        public void SetItem(string localStorageKey, bool value){
            LocalStorage.SetItem(localStorageKey, value, false);
        }
    }
}

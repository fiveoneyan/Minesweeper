/* Brief: IDConfig.json reader, support Editor Mode and Game Mode
 * Author: Komal
 * Date: "2019-07-10"
 */

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

namespace komal {
    public partial class KomalUtil {
        private Dictionary <string, object> m_ConfigDataDictionary =  new Dictionary<string, object>();
        public T GetConfig<T>(string path, string key){
            object _data;
            if (!m_ConfigDataDictionary.TryGetValue(key, out _data)) {
                // the key isn't in the dictionary.
                _data = this.ReadFromStreamAssets<T>(path);
                m_ConfigDataDictionary.Add(key, _data);
            }
            return (T)_data;
        }
    }    
}

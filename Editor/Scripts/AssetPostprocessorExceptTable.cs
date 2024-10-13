using System.Collections.Generic;
using System.Linq;
using LCHFramework.Editor.Utilities;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class AssetPostprocessorExceptTable<T> : ScriptableObject where T : Object
    {
        public static IEnumerable<T> Instances
        {
            get
            {
                if (_instances == null || _instancesTime != Time.frameCount)
                {
                    _instances = AssetDatabaseUtility.LoadAssetAtTypes<T>($"{nameof(T)}");
                    _instancesTime = Time.frameCount;
                }

                return _instances;
            }
        }
        private static IEnumerable<T> _instances;
        private static int _instancesTime;
        
        
        
        public List<string> exceptAssetPathPrefix;
        public List<string> exceptAssetNamePrefix;
        
        
        
        public bool IsExclude(string assetPath, string name1)
        {
            if (!exceptAssetPathPrefix.IsEmpty() && exceptAssetPathPrefix.Any(t => assetPath[..t.Length] == t))
                return true;

            if (!exceptAssetNamePrefix.IsEmpty() && exceptAssetNamePrefix.Any(t => name1[..t.Length] == t))
                return true;
                
            return false;
        }
    }
}

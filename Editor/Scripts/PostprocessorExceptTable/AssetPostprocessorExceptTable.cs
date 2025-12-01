using System.Collections.Generic;
using System.Linq;
using LCHFramework.Editor.Utilities;
using LCHFramework.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

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
                    _instances = AssetDatabaseUtility.LoadAssetsByType<T>($"{typeof(T).Name}");
                    _instancesTime = Time.frameCount;
                }
                
                return _instances;
            }
        }
        private static IEnumerable<T> _instances;
        private static int _instancesTime;
        
        
        
        public List<string> exceptAssetPathPrefix = new();
        public List<string> exceptAssetNamePrefix = new();
        
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(assetPath) && Selection.assetGUIDs.Exists()) assetPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            if (string.IsNullOrEmpty(assetPath)) return;

            assetPath = !Path.HasExtension(assetPath) ? assetPath : assetPath[..assetPath.LastIndexOf('/')];
            if (!exceptAssetPathPrefix.Contains(assetPath)) exceptAssetPathPrefix.Add(assetPath);
        }
#endif
        
        
        
        public bool IsExclude(string assetPath)
            => IsExclude(assetPath, assetPath[(assetPath.LastIndexOf('/') + 1)..assetPath.LastIndexOf('.')]);
        
        private bool IsExclude(string assetPath, string assetName)
        {
            var globalExceptAssetPathPrefix = new[] { "Assets/Packages", "Assets/Plugins" };
            if (!globalExceptAssetPathPrefix.IsEmpty() && globalExceptAssetPathPrefix.Any(t => t.Length <= assetPath.Length && assetPath[..t.Length] == t))
                return true;
                
            if (!exceptAssetPathPrefix.IsEmpty() && exceptAssetPathPrefix.Any(t => t.Length <= assetPath.Length && assetPath[..t.Length] == t))
                return true;
            
            if (!exceptAssetNamePrefix.IsEmpty() && exceptAssetNamePrefix.Any(t => t.Length <= assetName.Length && assetName[..t.Length] == t))
                return true;
            
            return false;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using LCHFramework.Editor.Utilities;
using LCHFramework.Extensions;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace LCHFramework.Editor
{
    [CreateAssetMenu(fileName = nameof(AssetPostprocessorExceptTable), menuName = "Scriptable Objects/LCHFramework/Asset Postprocessor Except Table")]
    public class AssetPostprocessorExceptTable : ScriptableObject
    {
        public static readonly ExceptAssetPrefix[] GlobalExceptAssetPathPrefix = { new("Assets/Editor Default Resources"), new("Assets/Packages"), new("Assets/Plugins") };
        
        
        public static IEnumerable<AssetPostprocessorExceptTable> Instances
        {
            get
            {
                if (_instances == null || _instancesTime != Time.frameCount)
                {
                    _instances = AssetDatabaseUtility.LoadAssetsByType<AssetPostprocessorExceptTable>(nameof(AssetPostprocessorExceptTable));
                    _instancesTime = Time.frameCount;
                }
                
                return _instances;
            }
        }
        private static IEnumerable<AssetPostprocessorExceptTable> _instances;
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
            if (!exceptAssetPathPrefix.IsEmpty() && exceptAssetPathPrefix.Any(t => new ExceptAssetPrefix(t).IsExclude(assetPath)))
                return true;
            
            if (!exceptAssetNamePrefix.IsEmpty() && exceptAssetNamePrefix.Any(t => new ExceptAssetPrefix(t).IsExclude(assetName)))
                return true;
            
            return false;
        }
        
        
        
        public class ExceptAssetPrefix
        {
            public ExceptAssetPrefix(string value) { t = value; }

            private readonly string t;
            
            public bool IsExclude(string b) => t.Length <= b.Length && t == b[..t.Length];
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using LCHFramework.Editor.Utilities;
using LCHFramework.Extensions;
using UnityEngine;

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
        
        
        
        public bool IsExclude(string assetPath) => !exceptAssetPathPrefix.IsEmpty() && exceptAssetPathPrefix.Any(t => new ExceptAssetPrefix(t).IsExclude(assetPath));
        
        
        
        public class ExceptAssetPrefix
        {
            public ExceptAssetPrefix(string value) { t = value; }
            
            private readonly string t;
            
            public bool IsExclude(string b) => t.Length <= b.Length && t == b[..t.Length];
        }
    }
}

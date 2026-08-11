using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCHFramework.Editor.Utilities;
using LCHFramework.Extensions;
using UnityEditor;
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
        
        
        
        public bool IsExclude(string assetPath) => !exceptAssetPathPrefix.IsEmpty() && exceptAssetPathPrefix.Any(t => new ExceptAssetPrefix(t, this).IsExclude(assetPath));
        
        
        
        public class ExceptAssetPrefix
        {
            public ExceptAssetPrefix(string value, AssetPostprocessorExceptTable assetPostprocessorExceptTableOrNull = null)
            {
                this.value = value;
                this.assetPostprocessorExceptTableOrNull = assetPostprocessorExceptTableOrNull;
            }
            
            private readonly string value;
            private readonly AssetPostprocessorExceptTable assetPostprocessorExceptTableOrNull;
            
            public bool IsExclude(string strB)
            {
                var strA = value;
                if (strA[..6] != "Assets" && assetPostprocessorExceptTableOrNull != null)
                    strA = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(assetPostprocessorExceptTableOrNull))}/{strA}";
                
                return strA.Length <= strB.Length && string.Compare(strA, strB[..strA.Length], StringComparison.OrdinalIgnoreCase) == 0;
            }
        }
    }
}

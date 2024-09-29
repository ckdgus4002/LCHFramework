using System.Collections.Generic;
using System.Linq;
using LCHFramework.Editor.Utilities;
using LCHFramework.Extensions;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    [CreateAssetMenu(fileName = nameof(AudioAssetPostprocessorExceptTable), menuName = nameof(AudioAssetPostprocessorExceptTable) + "/" + "Create")]
    public class AudioAssetPostprocessorExceptTable : ScriptableObject
    {
        public static IEnumerable<AudioAssetPostprocessorExceptTable> Instances
        {
            get
            {
                if (_instances == null || _instancesTime != Time.frameCount)
                {
                    _instances = AssetDatabaseUtility.LoadAssetAtTypes<AudioAssetPostprocessorExceptTable>($"{nameof(AudioAssetPostprocessorExceptTable)}");
                    _instancesTime = Time.frameCount;
                }

                return _instances;
            }
        }
        private static IEnumerable<AudioAssetPostprocessorExceptTable> _instances;
        private static int _instancesTime;
        
        
        
        public List<string> exceptAssetPathPrefix;
        public List<string> exceptAssetNamePrefix;
        
        
        
        public bool IsExclude(AudioImporter audioImporter)
        {
            if (!exceptAssetPathPrefix.IsEmpty() && exceptAssetPathPrefix.Any(t => audioImporter.assetPath[..t.Length] == t))
                return true;

            if (!exceptAssetNamePrefix.IsEmpty() && exceptAssetNamePrefix.Any(t => audioImporter.name[..t.Length] == t))
                return true;
                
            return false;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace LCHFramework.Editor
{
    [FilePath("Resources/LCHFramework/AudioAssetPostprocessorExceptTable.asset", FilePathAttribute.Location.ProjectFolder)]
    public class AudioAssetPostprocessorExceptTable : ScriptableSingleton<AudioAssetPostprocessorExceptTable>
    {
        public List<string> exceptAssetPathPrefix;
        public List<string> exceptAssetNamePrefix;
        
        
        
        public bool IsExclude(AudioImporter audioImporter)
        {
            if (exceptAssetPathPrefix.Any(t => audioImporter.assetPath[..t.Length] == t))
                return true;

            if (exceptAssetNamePrefix.Any(t => audioImporter.name[..t.Length] == t))
                return true;
                
            return false;
        }
    }
}

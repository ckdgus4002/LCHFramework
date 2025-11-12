using System.Linq;
using UnityEditor;

namespace LCHFramework.Editor
{
    public class SpritePostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var textureImporter = (TextureImporter)assetImporter;
            if (textureImporter.textureType != TextureImporterType.Sprite) return;
            if (SpritePostprocessorExceptTable.Instances.Any(t => t.IsExclude(textureImporter.assetPath))) return;
            
            textureImporter.spritePixelsPerUnit = 1;
            
            Debug.Log($"OnPostprocessAudio: {assetPath}");
        }
    }
}
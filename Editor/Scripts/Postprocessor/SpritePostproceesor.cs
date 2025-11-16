using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class SpritePostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var spriteImporter = (TextureImporter)assetImporter;
            if (spriteImporter.textureType != TextureImporterType.Sprite) return;
            if (SpritePostprocessorExceptTable.Instances.Any(t => t.IsExclude(spriteImporter.assetPath))) return;
            
            spriteImporter.spritePixelsPerUnit = 1;
            spriteImporter.textureCompression = TextureImporterCompression.Uncompressed;
            
            Debug.Log($"OnPostprocessAudio: {assetPath}");
        }
    }
}
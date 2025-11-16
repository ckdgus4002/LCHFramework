using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class SpritePostprocessor : AssetPostprocessor
    {
        private void OnPostprocessTexture(Texture2D texture)
        {
            var textureImporter = (TextureImporter)assetImporter;
            if (textureImporter.textureType != TextureImporterType.Sprite) return;
            
            OnPostprocessSprite(texture, textureImporter);
        }
        
        private void OnPostprocessSprite(Texture2D sprite, TextureImporter spriteImporter)
        {
            if (SpritePostprocessorExceptTable.Instances.Any(t => t.IsExclude(spriteImporter.assetPath))) return;
            
            spriteImporter.spritePixelsPerUnit = 1;
            spriteImporter.textureCompression = TextureImporterCompression.Uncompressed;
            
            Debug.Log($"{nameof(OnPostprocessSprite)}: {assetPath}");
        }
    }
}
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class SpritePostprocessor : AssetPostprocessor
    {
        private void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            if (!Enabled) return;
            if (AssetPostprocessorExceptTable.GlobalExceptAssetPathPrefix.Any(t => t.IsExclude(assetPath))) return;
            if (AssetPostprocessorExceptTable.Instances.Any(t => t.IsExclude(assetPath))) return;
            
            var spriteImporter = assetImporter as TextureImporter;
            if (spriteImporter == null || spriteImporter.textureType != TextureImporterType.Sprite) return;
            
            spriteImporter.spritePixelsPerUnit = 1;
            
            var textureImporterSettings = new TextureImporterSettings();
            spriteImporter.ReadTextureSettings(textureImporterSettings);
            if (spriteImporter.spriteBorder != Vector4.zero) textureImporterSettings.spriteMeshType = SpriteMeshType.FullRect;
            spriteImporter.SetTextureSettings(textureImporterSettings);
            
            if (!spriteImporter.assetPath.Contains("Unpacking", StringComparison.OrdinalIgnoreCase)) spriteImporter.textureCompression = TextureImporterCompression.Uncompressed;
            spriteImporter.compressionQuality = !spriteImporter.crunchedCompression ? 50 : 100;
            
            Debug.Log($"{nameof(OnPostprocessSprites)}: {assetPath}");
        }
    }
}
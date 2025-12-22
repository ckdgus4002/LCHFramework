using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class SpritePostprocessor : AssetPostprocessor
    {
        private void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            if (AssetPostprocessorExceptTable.GlobalExceptAssetPathPrefix.Any(t => t.IsExclude(assetPath))) return;
            if (AssetPostprocessorExceptTable.Instances.Any(t => t.IsExclude(assetPath))) return;

            var spriteImporter = assetImporter as TextureImporter;
            if (spriteImporter == null || spriteImporter.textureType != TextureImporterType.Sprite) return;
            
            spriteImporter.spritePixelsPerUnit = 1;
            
            var spriteImporterSettings = new TextureImporterSettings();
            spriteImporter.ReadTextureSettings(spriteImporterSettings);
            if (spriteImporter.spriteBorder != Vector4.zero) spriteImporterSettings.spriteMeshType = SpriteMeshType.FullRect;
            spriteImporter.SetTextureSettings(spriteImporterSettings);

            spriteImporter.textureCompression = TextureImporterCompression.Uncompressed;
            
            Debug.Log($"{nameof(OnPostprocessSprites)}: {assetPath}");
        }
    }
}
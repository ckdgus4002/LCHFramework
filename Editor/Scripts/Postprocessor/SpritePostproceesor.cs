using UnityEditor;

namespace LCHFramework.Editor
{
    public class SpritePostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var textureImporter = (TextureImporter)assetImporter;
            // if (SpritePostprocessorExceptTable.Instances.Any(t => t.IsExclude(textureImporter.assetPath))) return;
            if (textureImporter.textureType != TextureImporterType.Sprite) return;
            
            textureImporter.spritePixelsPerUnit = 1;
            
            Debug.Log($"OnPostprocessAudio: {assetPath}");
        }
    }
}
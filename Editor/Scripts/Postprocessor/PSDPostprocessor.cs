using System.Linq;
using UnityEditor;
using UnityEditor.U2D.PSD;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class PSDPostprocessor : AssetPostprocessor
    {
        private void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            var psdImporter = assetImporter as PSDImporter;
            if (psdImporter == null) return;
            
            OnPostprocessPSD(texture, sprites, psdImporter);
        }

        private void OnPostprocessPSD(Texture2D texture, Sprite[] sprites, PSDImporter psdImporter)
        {
            if (PSDPostprocessorExceptTable.GlobalExceptAssetPathPrefix.Any(t => t.IsExclude(assetPath))) return;
            if (PSDPostprocessorExceptTable.Instances.Any(t => t.IsExclude(assetPath))) return;
            
            psdImporter.spritePixelsPerUnit = 1;
            
            var defaultPlatformSettings = psdImporter.GetImporterPlatformSettings(BuildTarget.NoTarget);
            defaultPlatformSettings.textureCompression = TextureImporterCompression.Compressed;
            psdImporter.SetImporterPlatformSettings(defaultPlatformSettings);
            
            Debug.Log($"{nameof(OnPostprocessSprites)}: {assetPath}");
        }
    }
}
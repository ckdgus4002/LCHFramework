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
            if (AssetPostprocessorExceptTable.GlobalExceptAssetPathPrefix.Any(t => t.IsExclude(assetPath))) return;
            if (AssetPostprocessorExceptTable.Instances.Any(t => t.IsExclude(assetPath))) return;
            
            psdImporter.spritePixelsPerUnit = 1;
            
            var serializedObject = new SerializedObject(psdImporter);
            serializedObject.FindProperty("m_DocumentAlignment").enumValueIndex = (int)SpriteAlignment.Center;
            serializedObject.ApplyModifiedProperties();
            
            var defaultPlatformSettings = psdImporter.GetImporterPlatformSettings(BuildTarget.NoTarget);
            defaultPlatformSettings.textureCompression = TextureImporterCompression.Uncompressed;
            psdImporter.SetImporterPlatformSettings(defaultPlatformSettings);
            
            Debug.Log($"{nameof(OnPostprocessPSD)}: {assetPath}");
        }
    }
}
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace LCHFramework.Editor
{
    public class SpriteAtlasPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets.Where(t => Path.GetExtension(t).Contains(".spriteatlas")))
            {
                var spriteAtlasOrNull = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(importedAsset);
                if (spriteAtlasOrNull == null) return;

                OnPostprocessSpriteAtlas(spriteAtlasOrNull, importedAsset, AssetImporter.GetAtPath(importedAsset) as SpriteAtlasImporter);
            }
        }
        
        private static void OnPostprocessSpriteAtlas(SpriteAtlas spriteAtlas, string assetPath, SpriteAtlasImporter spriteAtlasImporterOrNull)
        {
            if (SpriteAtlasPostprocessorExceptTable.GlobalExceptAssetPathPrefix.Any(t => t.IsExclude(assetPath))) return;
            if (SpriteAtlasPostprocessorExceptTable.Instances.Any(t => t.IsExclude(assetPath))) return;
            
            var includeInBuild = AddressableAssetSettingsDefaultObject.Settings?.FindAssetEntry(AssetDatabase.AssetPathToGUID(assetPath)) == null;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetIncludeInBuild(includeInBuild);
            else spriteAtlasImporterOrNull.includeInBuild = includeInBuild;
            
            var packingSettings = spriteAtlas.GetPackingSettings();
            packingSettings.enableRotation = false;
            packingSettings.enableTightPacking = false;
            packingSettings.padding = 4;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetPackingSettings(packingSettings);
            else spriteAtlasImporterOrNull.packingSettings = packingSettings;
            
            var textureSettings = spriteAtlas.GetTextureSettings();
            textureSettings.generateMipMaps = false;
            textureSettings.sRGB = true;
            textureSettings.filterMode = FilterMode.Bilinear;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetTextureSettings(textureSettings);
            else spriteAtlasImporterOrNull.textureSettings = textureSettings;
            
            var defaultPlatformSettings = spriteAtlas.GetPlatformSettings("DefaultTexturePlatform");
            defaultPlatformSettings.maxTextureSize = 4096;
            defaultPlatformSettings.format = TextureImporterFormat.Automatic;
            defaultPlatformSettings.textureCompression = TextureImporterCompression.Compressed;
            defaultPlatformSettings.crunchedCompression = false;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetPlatformSettings(defaultPlatformSettings);
            else spriteAtlasImporterOrNull.SetPlatformSettings(defaultPlatformSettings);
            
            foreach (var platformGroup in Application.PlatformGroups)
            {
                var platformSettings = spriteAtlas.GetPlatformSettings($"{platformGroup}");
                platformSettings.overridden = false;
                if (spriteAtlasImporterOrNull == null) spriteAtlas.SetPlatformSettings(platformSettings);
                else spriteAtlasImporterOrNull.SetPlatformSettings(platformSettings);
            }
            
            Debug.Log($"{nameof(OnPostprocessSpriteAtlas)}: {assetPath}");
        }
    }
}


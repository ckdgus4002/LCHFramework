using System.IO;
using System.Linq;
using System.Reflection;
using LCHFramework.Editor.Utilities;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Editor
{
    public class SpriteAtlasPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets.Where(t => Path.GetExtension(t) == "spriteatlasv2"))
            {
                var spriteAtlasImporterOrNull = AssetImporter.GetAtPath(importedAsset) as SpriteAtlasImporter;
                var spriteAtlasOrNull = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(importedAsset);
                if (spriteAtlasImporterOrNull == null || spriteAtlasOrNull == null) return;

                OnPostprocessSpriteAtlas(spriteAtlasImporterOrNull, spriteAtlasOrNull);
            }
        }

        private static void OnPostprocessSpriteAtlas(SpriteAtlasImporter spriteAtlasImporter, SpriteAtlas spriteAtlas)
        {
            if (SpriteAtlasPostprocessorExceptTable.Instances.Any(t => t.IsExclude(spriteAtlasImporter.assetPath))) return;

            const string getIncludeInBuildMethodName = "GetIncludeInBuild";
            var getIncludeInBuildOrNull = AssemblyUtility.InvokeMethod($"LCHFramework.Editor.{nameof(SpriteAtlasPostprocessor)}_{getIncludeInBuildMethodName}", getIncludeInBuildMethodName, BindingFlags.NonPublic | BindingFlags.Static, null, new object[] { spriteAtlasImporter, spriteAtlas });
            var includeInBuild = getIncludeInBuildOrNull != null ? (bool)getIncludeInBuildOrNull : spriteAtlasImporter.includeInBuild;
            spriteAtlasImporter.includeInBuild = includeInBuild;
            
            var packingSettings = spriteAtlasImporter.packingSettings;
            packingSettings.enableRotation = false;
            packingSettings.enableTightPacking = false;
            packingSettings.enableAlphaDilation = false;
            packingSettings.padding = 4;
            spriteAtlasImporter.packingSettings = packingSettings;

            var textureSettings = spriteAtlasImporter.textureSettings;
            textureSettings.generateMipMaps = false;
            textureSettings.sRGB = true;
            textureSettings.filterMode = FilterMode.Bilinear;
            spriteAtlasImporter.textureSettings = textureSettings;
            
            foreach (var platformGroup in LCHFramework.PlatformGroups)
            {
                var platformSettings = spriteAtlasImporter.GetPlatformSettings($"{platformGroup}");
                platformSettings.overridden = false;
                spriteAtlasImporter.SetPlatformSettings(platformSettings);
            }
            
            spriteAtlas.Remove(spriteAtlas.GetPackables());
            const string getPackableTargetsMethodName = "GetPackableTargets";
            var getPackableTargetsOrNull = AssemblyUtility.InvokeMethod($"LCHFramework.Editor.{nameof(SpriteAtlasPostprocessor)}_{getPackableTargetsMethodName}", getPackableTargetsMethodName, BindingFlags.NonPublic | BindingFlags.Static, null, new object[] { spriteAtlasImporter, spriteAtlas });
            var packableTargets = getPackableTargetsOrNull != null ? (Object[])getPackableTargetsOrNull : new[] { AssetDatabase.LoadAssetAtPath<Object>(spriteAtlasImporter.assetPath[..spriteAtlasImporter.assetPath.LastIndexOf('/')]) }; 
            spriteAtlas.Add(packableTargets);
            SpriteAtlasUtility.PackAtlases(new[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
            SyncPlatformSettings();
            
            Debug.Log($"OnPostprocessSpriteAtlas: {spriteAtlasImporter.assetPath}");
        }
        
        /// <remarks>
        /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/2D/SpriteAtlas/SpriteAtlasInspector.cs#L253
        /// </remarks>
        private static void SyncPlatformSettings()
        {
        }
    }
}


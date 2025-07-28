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
            foreach (var importedAsset in importedAssets.Where(t => Path.GetExtension(t).Contains(".spriteatlas")))
            {
                var spriteAtlasOrNull = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(importedAsset);
                if (spriteAtlasOrNull == null) return;

                OnPostprocessSpriteAtlas(spriteAtlasOrNull, importedAsset, AssetImporter.GetAtPath(importedAsset) as SpriteAtlasImporter);
            }
        }
        
        private static void OnPostprocessSpriteAtlas(SpriteAtlas spriteAtlas, string spriteAtlasPath, SpriteAtlasImporter spriteAtlasImporterOrNull)
        {
            if (SpriteAtlasPostprocessorExceptTable.Instances.Any(t => t.IsExclude(spriteAtlasPath))) return;

            const string getIncludeInBuildMethodName = "GetIncludeInBuild";
            var getIncludeInBuildOrNull = AssemblyUtility.InvokeMethod($"{nameof(LCHFramework)}.Editor.{nameof(SpriteAtlasPostprocessor)}_{getIncludeInBuildMethodName}", getIncludeInBuildMethodName, BindingFlags.NonPublic | BindingFlags.Static, null, new object[] { spriteAtlasImporterOrNull, spriteAtlas });
            var includeInBuild = getIncludeInBuildOrNull == null || (bool)getIncludeInBuildOrNull;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetIncludeInBuild(includeInBuild);
            else spriteAtlasImporterOrNull.includeInBuild = includeInBuild;

            var packingSettings = spriteAtlas.GetPackingSettings();
            packingSettings.enableRotation = false;
            packingSettings.enableTightPacking = false;
            packingSettings.enableAlphaDilation = true;
            packingSettings.padding = 4;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetPackingSettings(packingSettings);
            else spriteAtlasImporterOrNull.packingSettings = packingSettings;

            var textureSettings = spriteAtlas.GetTextureSettings();
            textureSettings.generateMipMaps = false;
            textureSettings.sRGB = true;
            textureSettings.filterMode = FilterMode.Bilinear;
            if (spriteAtlasImporterOrNull == null) spriteAtlas.SetTextureSettings(textureSettings);
            else spriteAtlasImporterOrNull.textureSettings = textureSettings;
            
            foreach (var platformGroup in Application.PlatformGroups)
            {
                var platformSettings = spriteAtlas.GetPlatformSettings($"{platformGroup}");
                platformSettings.overridden = false;
                if (spriteAtlasImporterOrNull == null) spriteAtlas.SetPlatformSettings(platformSettings);
                else spriteAtlasImporterOrNull.SetPlatformSettings(platformSettings);
            }
            
            spriteAtlas.Remove(spriteAtlas.GetPackables());
            const string getPackableTargetsMethodName = "GetPackableTargets";
            var getPackableTargetsOrNull = AssemblyUtility.InvokeMethod($"{nameof(LCHFramework)}.Editor.{nameof(SpriteAtlasPostprocessor)}_{getPackableTargetsMethodName}", getPackableTargetsMethodName, BindingFlags.NonPublic | BindingFlags.Static, null, new object[] { spriteAtlasImporterOrNull, spriteAtlas });
            var packableTargets = getPackableTargetsOrNull == null ? new[] { AssetDatabase.LoadAssetAtPath<Object>(spriteAtlasPath[..spriteAtlasPath.LastIndexOf('/')]) } : (Object[])getPackableTargetsOrNull; 
            spriteAtlas.Add(packableTargets);
            SpriteAtlasUtility.PackAtlases(new[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
            SyncPlatformSettings();
            
            Debug.Log($"OnPostprocessSpriteAtlas: {spriteAtlasPath}");
        }
        
        /// <remarks>
        /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/2D/SpriteAtlas/SpriteAtlasInspector.cs#L253
        /// </remarks>
        private static void SyncPlatformSettings()
        {
        }
    }
}


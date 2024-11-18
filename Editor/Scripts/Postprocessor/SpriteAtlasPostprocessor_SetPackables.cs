using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace LCHFramework.Editor
{
    public partial class SpriteAtlasPostprocessor
    {
        /// <remarks> Invoke by Reflection. </remarks>
        private static void SetPackables(SpriteAtlasImporter spriteAtlasImporter, SpriteAtlas spriteAtlas)
        {
            spriteAtlas.Remove(spriteAtlas.GetPackables());
            spriteAtlas.Add(new[] { AssetDatabase.LoadAssetAtPath<Object>(spriteAtlasImporter.assetPath[..spriteAtlasImporter.assetPath.LastIndexOf('/')]) });
        }
    }
}
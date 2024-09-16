using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public static class AssetDatabaseUtility
    {
        public static T[] LoadAssetAtTypes<T>(string filter, string[] searchInFolders = null) where T : ScriptableObject
            => AssetDatabase.FindAssets(filter, searchInFolders)
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(t => t != null)
                .ToArray();
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Utilities
{
    public static class AssetDatabaseUtility
    {
        public static T[] LoadAssetsByType<T>(string filter, string[] searchInFolders = null) where T : Object
            => _LoadAssetsByType<T>(filter, searchInFolders).Where(t => t != null).ToArray();

        public static T LoadAssetByType<T>(string filter, string[] searchInFolders = null) where T : Object
            => _LoadAssetsByType<T>(filter, searchInFolders).FirstOrDefault(t => t != null);

        private static IEnumerable<T> _LoadAssetsByType<T>(string filter, string[] searchInFolders = null) where T : Object
            => AssetDatabase.FindAssets(filter, searchInFolders)
                .Select(guid => AssetDatabase.LoadAssetByGUID<T>(new GUID(guid)));
    }
}

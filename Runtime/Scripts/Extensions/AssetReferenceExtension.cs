using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#else 
using System.Linq;
#endif

namespace LCHFramework.Extensions
{
    public static class AssetReferenceExtension
    {
        public static string GetAddress(this AssetReference asset)
        {
#if UNITY_EDITOR
            return AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(asset.AssetGUID).address;
#else
            return Addressables.LoadResourceLocationsAsync(asset.RuntimeKey).WaitForCompletion().First().PrimaryKey;
#endif
        }
    }
}
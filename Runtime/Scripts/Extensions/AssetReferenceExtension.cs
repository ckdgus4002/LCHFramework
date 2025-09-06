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
            // if (asset == null) return string.Empty;
#if UNITY_EDITOR
            if (AddressableAssetSettingsDefaultObject.Settings == null) return string.Empty;

            var assetEntry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(asset.AssetGUID);
            if (assetEntry == null) return string.Empty;
            
            return assetEntry.address;
#else
            var resourceLocations = Addressables.LoadResourceLocationsAsync(asset.RuntimeKey).WaitForCompletion();
            if (resourceLocations.IsEmpty()) return string.Empty;

            return resourceLocations.First().PrimaryKey;
#endif
        }
    }
}
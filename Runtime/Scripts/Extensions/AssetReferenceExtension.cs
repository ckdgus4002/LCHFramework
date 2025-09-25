using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
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
            Assert.IsNull(asset);
#if UNITY_EDITOR
            if (AddressableAssetSettingsDefaultObject.Settings == null) return "";

            var assetEntry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(asset.AssetGUID);
            if (assetEntry == null) return "";
            
            return assetEntry.address;
#else
            var resourceLocations = Addressables.LoadResourceLocationsAsync(asset.RuntimeKey).WaitForCompletion();
            if (resourceLocations.IsEmpty()) return "";

            return resourceLocations.First().PrimaryKey;
#endif
        }
    }
}
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
        public static string GetAddress(this AssetReference assetReference)
        {
            Assert.IsNotNull(assetReference);
            
#if UNITY_EDITOR
            var assetEntry = AddressableAssetSettingsDefaultObject.Settings == null ? null : AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(assetReference.AssetGUID);
            return assetEntry == null ? "" : assetEntry.address;
#else
            return Addressables.LoadResourceLocationsAsync(assetReference.RuntimeKey).WaitForCompletion().FirstOrDefault().PrimaryKey;
#endif
        }
        
        public static bool IsEmpty(this AssetReference assetReference) => !UnityEngine.Application.isPlaying ? assetReference.AssetGUID == string.Empty : !assetReference.RuntimeKeyIsValid();
    }
}
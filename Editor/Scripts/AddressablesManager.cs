using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets;

namespace LCHFramework.Editor
{
    public static class AddressablesManager
    {
        /// <remarks>
        /// Used at AddressableAssetSettings.CustomContentStateBuildPath.
        /// </remarks>
        public static string AddressableContentStateBuildPath
        {
            get
            {
                var profileId = AddressableAssetSettingsDefaultObject.Settings.activeProfileId;
                var profileName = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileName(profileId);
                var profileIsDefault = profileName == "Default";
                var defaultContentStateBuildPath = $"{AddressableAssetSettingsDefaultObject.kDefaultConfigFolder}/{PlatformMappingService.GetPlatformPathSubFolder()}";
                var customContentStateBuildPath = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetValueByName(profileId, AddressableAssetSettings.kRemoteBuildPath); 
                return profileIsDefault ? defaultContentStateBuildPath : customContentStateBuildPath;
            }
        }
    }
}
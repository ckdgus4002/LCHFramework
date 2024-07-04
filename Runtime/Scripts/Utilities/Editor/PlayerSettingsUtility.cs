#if UNITY_EDITOR
using UnityEditor;

namespace LCHFramework.Utilities
{
    public static class PlayerSettingsUtility
    {
        public static bool TryGetBuildNumber(out string result) => (result = GetBuildNumber()) != string.Empty;

        public static string GetBuildNumber()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    return PlayerSettings.macOS.buildNumber;
                case BuildTarget.Android:
                    return $"{PlayerSettings.Android.bundleVersionCode}";
                case BuildTarget.iOS:
                    return PlayerSettings.iOS.buildNumber;
                case BuildTarget.tvOS:
                    return PlayerSettings.tvOS.buildNumber;
#if UNITY_6000_0_OR_NEWER
                case BuildTarget.VisionOS:
                    return PlayerSettings.VisionOS.buildNumber;
#else
                case BuildTarget.VisionOS:
                    return PlayerSettings.Bratwurst.buildNumber;
#endif                
                default:
                    return string.Empty;
            }
        }
    }
}
#endif
using System;
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
#if !UNITY_6000_0_OR_NEWER
                case BuildTarget.Bratwurst:
                    return PlayerSettings.Bratwurst.buildNumber;
#else
                case BuildTarget.VisionOS:
                    return PlayerSettings.VisionOS.buildNumber;
#endif                
                default:
                    return string.Empty;
            }
        }
        
        public static void SetBuildNumber(string value)
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    PlayerSettings.macOS.buildNumber = value;
                    break;
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode = Convert.ToInt32(value);
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = value;
                    break;
                case BuildTarget.tvOS:
                    PlayerSettings.tvOS.buildNumber = value;
                    break;
#if !UNITY_6000_0_OR_NEWER
                case BuildTarget.Bratwurst:
                    PlayerSettings.Bratwurst.buildNumber = value;
                    break;
#else
                case BuildTarget.VisionOS:
                    PlayerSettings.VisionOS.buildNumber = value;
                    break;
#endif
                default:
                    break;
            }
        }
    }
}
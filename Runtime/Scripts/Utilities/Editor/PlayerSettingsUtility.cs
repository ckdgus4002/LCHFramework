#if UNITY_EDITOR
using UnityEditor;

namespace LCHFramework.Utilities
{
    public static class PlayerSettingsUtility
    {
        public static bool TryGetBuildNumber(out string result)
        {
            result = GetBuildNumber();
            return result != string.Empty;
        }

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
                case BuildTarget.Bratwurst:
                    return PlayerSettings.Bratwurst.buildNumber;
                default:
                    return string.Empty;
            }
        }
    }
}
#endif
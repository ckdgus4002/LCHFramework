using UnityEditor;

namespace LCHFramework.Editor.Utils
{
    public static class VersionCodeUtility
    {
        public static bool TryGetVersionCode(out string result)
        {
            result = GetVersionCode();
            return result != string.Empty;
        }

        public static string GetVersionCode() => EditorUserBuildSettings.activeBuildTarget switch
        {
            BuildTarget.StandaloneOSX => PlayerSettings.macOS.buildNumber,
            BuildTarget.Android => $"{PlayerSettings.Android.bundleVersionCode}",
            BuildTarget.iOS => PlayerSettings.iOS.buildNumber,
            _ => string.Empty
        };
    }
}

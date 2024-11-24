#if UNITY_IOS
using LCHFramework.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace LCHFramework.Editor
{
    public static class PBXProjectPostProcessBuilder
    {
        [PostProcessBuild]
        private static void Initialize(BuildTarget buildTarget, string pathToBuiltProject)
        {
            var pbxProject = new PBXProject();
            var pbxProjectPath = $"{pathToBuiltProject}/Unity iPhone.xcodeproj/project.pbxproj";
            pbxProject.ReadFromFile(pbxProjectPath);
            // pbxProject.SetBuildProperty(pbxProject.GetUnityMainTargetGuid(), "ENABLE_BITCODE", "NO");
            pbxProject.WriteToFile(pbxProjectPath);

            Debug.Log("PBXProject is writed");
        }
    }
}
#endif

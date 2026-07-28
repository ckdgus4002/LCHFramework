#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace LCHFramework.Editor
{
    public static class PlistDocumentPostProcessBuilder
    {
        [PostProcessBuild]
        private static void Initialize(BuildTarget buildTarget, string pathToBuiltProject)
        {
            var plistDocument = new PlistDocument();
            var plistDocumentPath = $"{pathToBuiltProject}/Info.plist";
            plistDocument.ReadFromFile(plistDocumentPath);
                 
            plistDocument.root.SetBoolean("ITSAppUsesNonExemptEncryption", false); // 앱이 수출 규정 대상이 아님을 설정합니다. 
            plistDocument.WriteToFile(plistDocumentPath);
            
            Debug.Log("PlistDocument is wrote.");
        }
    }
}
#endif

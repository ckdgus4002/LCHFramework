#if UNITY_IOS
using LCHFramework.Utilities;
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
            // plistDocument.root.values.Remove("UIApplicationExitsOnSuspend", out _); // 지원 만료되는 기능입니다. 키를 제거하여 Appstore에서 수신되는 issue 메일을 방지합니다. 
            plistDocument.WriteToFile(plistDocumentPath);
            
            Debug.Log("PlistDocument is writed");
        }
    }
}
#endif

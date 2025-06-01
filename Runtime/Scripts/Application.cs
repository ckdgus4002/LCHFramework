#if !UNITY_EDITOR
using System.IO;
#else
using LCHFramework.Utilities.Editor;
using UnityEditor;
#endif

namespace LCHFramework
{
    public class Application
    {
        public static string BuildNumber
        {
            get
            {
#if UNITY_EDITOR
                return PlayerSettingsUtility.GetBuildNumber();
#else
                return File.Exists(LCHFramework.BuildNumberInfoFilePath)
                    ? File.ReadAllText(LCHFramework.BuildNumberInfoFilePath)
                    : string.Empty;
#endif
            }
        }
        
        public static bool isEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
        
        public static bool IsSimulator
        {
            get
            {
#if UNITY_EDITOR
                return !UnityEngine.Application.isEditor;
#else
                return false;
#endif
            }
        }



        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
#if !UNITY_EDITOR
using System.IO;
#if UNITY_ANDROID
using UnityEngine;
#endif
#endif
using LCHFramework.Utilities.Editor;
using UnityEditor;

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
        
#if !UNITY_EDITOR && UNITY_ANDROID
        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                _unityPlayerClass ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _currentActivity ??= _unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                return _currentActivity;
            }
        }
        private static AndroidJavaObject _unityPlayerClass;
        private static AndroidJavaObject _currentActivity;
#endif
        
        
        
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
using System;
using UnityEngine;
#if UNITY_EDITOR
using LCHFramework.Utilities.Editor;
using UnityEditor;
#endif

namespace LCHFramework
{
    public static class Application
    {
        public static bool isEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return UnityEngine.Application.isEditor;
#endif
            }
        }
        
        public static Version version => _version ??= new Version(UnityEngine.Application.version);
        private static Version _version;
        
        public static string BuildNumber
        {
            get
            {
                if (_buildNumber == null)
                {
#if UNITY_EDITOR
                    return PlayerSettingsUtility.GetBuildNumber();
#elif UNITY_ANDROID
                    return $"{(AndroidApiLevel < 28 ? AndroidPackageInfo.Get<int>("versionCode") : AndroidPackageInfo.Get<long>("longVersionCode"))}";
#elif UNITY_IOS
                    return GetIOSBuildNumber();
#else
                    return string.Empty;
#endif   
                }

                return _buildNumber;
            }
        }
        private static string _buildNumber;
        
#if !UNITY_EDITOR && UNITY_ANDROID
        public static int AndroidApiLevel
        {
            get
            {
                if (_androidApiLevel < -1)
                {
                    using var buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
                    _androidApiLevel = buildVersion.GetStatic<int>("SDK_INT");
                }

                return _androidApiLevel;
            }
        }
        private static int _androidApiLevel = -2;
        
        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                if (_currentActivity == null)
                {
                    var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                    unityPlayerClass.Dispose();
                }
                
                return _currentActivity;
            }
        }
        private static AndroidJavaObject _currentActivity;

        private static AndroidJavaObject AndroidPackageInfo
        {
            get
            {
                if (_androidPackageInfo == null)
                {
                    using var packageManager = CurrentActivity.Call<AndroidJavaObject>("getPackageManager");
                    var packageName = CurrentActivity.Call<string>("getPackageName");
                    _androidPackageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
                }

                return _androidPackageInfo;
            }
        }
        private static AndroidJavaObject _androidPackageInfo;
#endif
        
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod() => LCHFramework.onApplicationQuit += OnApplicationQuit;
        
        private static void OnApplicationQuit()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            _currentActivity.Dispose();
            _androidPackageInfo.Dispose();
#endif
        }
        
        
        
        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
        
#if !UNITY_EDITOR && UNITY_IOS
    [DllImport("__Internal")]
    private static extern string _GetIOSBuildNumber();
#endif
    }
}
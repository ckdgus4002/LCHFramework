using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#elif UNITY_IOS
using UnityEngine.iOS;
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
        
        public static int BuildNumber
        {
            get
            {
#if UNITY_EDITOR
                switch (EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.StandaloneOSX:
                        return Convert.ToInt32(PlayerSettings.macOS.buildNumber);
                    case BuildTarget.Android:
                        return PlayerSettings.Android.bundleVersionCode;
                    case BuildTarget.iOS:
                        return Convert.ToInt32(PlayerSettings.iOS.buildNumber);
                    case BuildTarget.tvOS:
                        return Convert.ToInt32(PlayerSettings.tvOS.buildNumber);
#if UNITY_2023_2_OR_NEWER && !UNITY_6000_0_OR_NEWER
                    case BuildTarget.Bratwurst:
                        return Convert.ToInt32(PlayerSettings.Bratwurst.buildNumber);
#elif UNITY_6000_0_OR_NEWER
                    case BuildTarget.VisionOS:
                        return Convert.ToInt32(PlayerSettings.VisionOS.buildNumber);
#endif
                    default:
                        return -1;
                }
#else
                if (_buildNumber < -1)
                {
#elif UNITY_ANDROID
                    _buildNumber = (AndroidApiLevel < 28 ? AndroidPackageInfo.Get<int>("versionCode") : Convert.ToInt32(AndroidPackageInfo.Get<long>("longVersionCode"));
#elif UNITY_IOS
                    _buildNumber = Convert.ToInt32(GetIOSBuildNumber());
#else
                    _buildNumber = -1;
                }

                return _buildNumber;
#endif
            }
            set
            {
#if UNITY_EDITOR
                switch (EditorUserBuildSettings.activeBuildTarget)
                {
                    case BuildTarget.StandaloneOSX:
                        PlayerSettings.macOS.buildNumber = $"{value}";
                        break;
                    case BuildTarget.Android:
                        PlayerSettings.Android.bundleVersionCode = value;
                        break;
                    case BuildTarget.iOS:
                        PlayerSettings.iOS.buildNumber = $"{value}";
                        break;
                    case BuildTarget.tvOS:
                        PlayerSettings.tvOS.buildNumber = $"{value}";
                        break;
#if UNITY_2023_2_OR_NEWER && !UNITY_6000_0_OR_NEWER 
                    case BuildTarget.Bratwurst:
                        PlayerSettings.Bratwurst.buildNumber = $"{value}";
                        break;
#elif UNITY_6000_0_OR_NEWER
                    case BuildTarget.VisionOS:
                        PlayerSettings.VisionOS.buildNumber = $"{value}";
                        break;
#endif
                }
#else
                if (BuildNumber < 0) return;

                _buildNumber = value;
#endif
            }
        }
#if !UNITY_EDITOR
        private static int _buildNumber = -2;
#endif
        
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
        
#elif !UNITY_EDITOR && UNITY_IOS
        public static Version IOSVersion => _iOSVersion ??= new Version(Device.systemVersion);
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
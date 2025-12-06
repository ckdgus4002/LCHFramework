#if (!UNITY_EDITOR && UNITY_ANDROID) || UNITY_IOS || UNITY_WEBGL
using LCHFramework.Utilities;
#endif
using System;
#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
#if !UNITY_EDITOR && UNITY_IOS
using UnityEngine.iOS;
#endif

namespace LCHFramework
{
    public static class Application
    {
        public static Version version => _version ??= new Version(UnityEngine.Application.version);
        private static Version _version;
        
        public static long BuildNumber
        {
            get
            {
#if UNITY_EDITOR && UNITY_STANDALONE_OSX
                return Convert.ToInt64(PlayerSettings.macOS.buildNumber);
#elif UNITY_EDITOR && UNITY_ANDROID
                return PlayerSettings.Android.bundleVersionCode;
#elif UNITY_EDITOR && UNITY_IOS
                return Convert.ToInt64(PlayerSettings.iOS.buildNumber);
#elif UNITY_EDITOR && UNITY_TVOS
                return Convert.ToInt64(PlayerSettings.tvOS.buildNumber);
#elif UNITY_EDITOR && UNITY_VISIONOS && UNITY_2023_2_OR_NEWER && !UNITY_6000_0_OR_NEWER
                return Convert.ToInt64(PlayerSettings.Bratwurst.buildNumber);
#elif UNITY_EDITOR && UNITY_VISIONOS && UNITY_6000_0_OR_NEWER
                return Convert.ToInt64(PlayerSettings.VisionOS.buildNumber);
#elif !UNITY_EDITOR && UNITY_ANDROID
                if (-2 < _buildNumber) return _buildNumber;

                using var packageManager = CurrentActivity.Call<AndroidJavaObject>("getPackageManager");
                var packageName = CurrentActivity.Call<string>("getPackageName");
                AndroidJavaObject packageInfo = null;
                if (AndroidApiLevel < 33)
                    packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
                else
                {
                    using var packageInfoFlags = new AndroidJavaClass("android.content.pm.PackageManager$PackageInfoFlags");
                    using var packageInfoFlagsOf = packageInfoFlags.CallStatic<AndroidJavaObject>("of", 0L);
                    packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, packageInfoFlagsOf);
                }
                _buildNumber = AndroidApiLevel < 28 ? packageInfo.Get<int>("versionCode") : packageInfo.Call<long>("getLongVersionCode");
                packageInfo.Dispose();
                return _buildNumber;
#elif !UNITY_EDITOR && UNITY_IOS
                if (-2 < _buildNumber) return _buildNumber;

                _buildNumber = !long.TryParse(Marshal.PtrToStringAnsi(GetiOSBuildNumber()), out var result) ? -1 : result;
                return _buildNumber;
#else
                return -1;
#endif
            }
#if UNITY_EDITOR
            set
            {
#if UNITY_STANDALONE_OSX
                PlayerSettings.macOS.buildNumber = $"{value}";
#elif UNITY_ANDROID
                PlayerSettings.Android.bundleVersionCode = (int)Math.Clamp(value, int.MinValue, int.MaxValue);
#elif UNITY_IOS
                PlayerSettings.iOS.buildNumber = $"{value}";
#elif UNITY_TVOS
                PlayerSettings.tvOS.buildNumber = $"{value}";
#elif UNITY_VISIONOS && UNITY_2023_2_OR_NEWER && !UNITY_6000_0_OR_NEWER
                PlayerSettings.Bratwurst.buildNumber = $"{value}";
#elif UNITY_VISIONOS && UNITY_6000_0_OR_NEWER
                PlayerSettings.VisionOS.buildNumber = $"{value}";
#endif
            }
#endif
        }
#if !UNITY_EDITOR
        private static long _buildNumber = -2;
#endif
        
        public static int AndroidApiLevel
        {
            get
            {
                if (_androidApiLevel < -1)
                {
#if UNITY_EDITOR || !UNITY_ANDROID
                    _androidApiLevel = -1;
#else
                    using var buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
                    _androidApiLevel = buildVersion.GetStatic<int>("SDK_INT");
#endif
                }
                
                return _androidApiLevel;
            }
        }
        private static int _androidApiLevel = -2;
        
#if !UNITY_EDITOR && UNITY_ANDROID
        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                if (_currentActivity == null)
                {
                    using var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                }
                
                return _currentActivity;
            }
        }
        private static AndroidJavaObject _currentActivity;
#endif
        
        public static Version IOSVersion => _iOSVersion == null
#if UNITY_EDITOR || !UNITY_IOS
            ? _iOSVersion = new Version()
#else
            ? _iOSVersion = new Version(Device.systemVersion)
#endif
            : _iOSVersion;
        private static Version _iOSVersion;
        
        public static bool IsIPad => UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer && SystemInfo.deviceModel.Contains("iPad", StringComparison.OrdinalIgnoreCase);
        
        
        
        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitializeOnLoadMethod()
        {
            Observable.OnceApplicationQuit().Subscribe(_ =>
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                IDisposableUtility.DisposeAndSetDefault(ref _currentActivity);
#endif
            });
        }
        
        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

        public static async Awaitable<bool> RequestUserCameraPermissionAsync()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
                if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) return false;
                
                await Awaitable.WaitForSecondsAsync(0.1f);
                return true;
            }
            else
                return true;
#else // UNITY_IOS || UNITY_WEBGL
            if (!UnityEngine.Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                await UnityEngine.Application.RequestUserAuthorization(UserAuthorization.WebCam);
                if (!UnityEngine.Application.HasUserAuthorization(UserAuthorization.WebCam)) return false;
 
                return true;
            }
            else
                return true;
#endif
        }
        
#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        private static extern IntPtr GetiOSBuildNumber();
#endif
    }
}
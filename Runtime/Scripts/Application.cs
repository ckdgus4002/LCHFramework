using System;
using System.Linq;
using LCHFramework.Utilities;
using UniRx;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
using UnityEngine.iOS;
#endif
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Recorder;
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
        
        public static bool IsLargeScreenMobile
        {
            get
            {
                if (!UnityEngine.Device.Application.isMobilePlatform) return false;

                var logicalScale = UnityEngine.Device.Application.platform == RuntimePlatform.Android
                            ? UnityEngine.Device.Screen.dpi <= 0 ? 1f : UnityEngine.Device.Screen.dpi / 160f
                            : UnityEngine.Device.Screen.dpi <= 0 ? 2f : Mathf.Max(1f, Mathf.Round(UnityEngine.Device.Screen.dpi / 163f));
                var smallestScreenWidthDp = Mathf.FloorToInt(Mathf.Min(UnityEngine.Device.Screen.width, UnityEngine.Device.Screen.height) / logicalScale);
                return 600f <= smallestScreenWidthDp;
            }
        }
        
        public static bool IsRecording
        {
            get
            {
#if UNITY_EDITOR
                return Resources.FindObjectsOfTypeAll<RecorderWindow>().Any(t => t.IsRecording());
#else
                return false;
#endif
            }
        }
        
        
        
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
        
        public enum RequestUserPermissionResult
        {
            None = -1,
            Denied = 0,
            DeniedAndDontAskAgain = 1,
            RequestDismissed = 2,
            Granted = 3,
        }
        
        public static async Awaitable<RequestUserPermissionResult> RequestUserPermissionAsync(UserAuthorization userAuthorization)
        {
            var result = RequestUserPermissionResult.None;
#if UNITY_ANDROID
            var permission = userAuthorization == UserAuthorization.WebCam ? Permission.Camera : Permission.Microphone;
            if (!HasUserAuthorization(permission))
            {
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += _ =>
                {
#if UNITY_EDITOR
                    result = RequestUserPermissionResult.Denied;
#else
                    result = CurrentActivity.Call<bool>("shouldShowRequestPermissionRationale", permission) ? RequestUserPermissionResult.Denied : RequestUserPermissionResult.DeniedAndDontAskAgain;
#endif
                };
                callbacks.PermissionRequestDismissed += _ => result = RequestUserPermissionResult.RequestDismissed;
                callbacks.PermissionGranted += _ => result = RequestUserPermissionResult.Granted;
                Permission.RequestUserPermission(permission, callbacks);
                await AwaitableUtility.WaitUntil(() => result != RequestUserPermissionResult.None);

                if (RequestUserPermissionResult.Granted <= result) await Awaitable.NextFrameAsync();
            }
            else
                result = RequestUserPermissionResult.Granted;
#else
            if (!HasUserAuthorization(userAuthorization))
            {
                await UnityEngine.Application.RequestUserAuthorization(userAuthorization);
                result = !HasUserAuthorization(userAuthorization) ? RequestUserPermissionResult.Denied : RequestUserPermissionResult.Granted;
            }
            else
                result = RequestUserPermissionResult.Granted;
#endif
            return result;
        }
        
        public static bool HasUserAuthorization(string permission)
        {
#if UNITY_ANDROID
            return Permission.HasUserAuthorizedPermission(permission);
#else
            return HasUserAuthorization(permission switch
            {
                /*Permission.Camera*/"android.permission.CAMERA" => UserAuthorization.WebCam,
                /*Permission.Microphone*/"android.permission.RECORD_AUDIO" => UserAuthorization.Microphone,
                _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null),
            });
#endif
        }
        
        public static bool HasUserAuthorization(UserAuthorization userAuthorization)
        {
#if UNITY_ANDROID
            return HasUserAuthorization(userAuthorization switch
            {
                UserAuthorization.WebCam => Permission.Camera,
                UserAuthorization.Microphone => Permission.Microphone,
                _ => throw new ArgumentOutOfRangeException(nameof(userAuthorization), userAuthorization, null),
            });
#else
            return UnityEngine.Application.HasUserAuthorization(userAuthorization);
#endif
        }
        
        public static async Awaitable OpenAppSettingsAsync()
        {
            var step = 0;   // 0: 이탈 대기, 1: 설정 화면으로 이탈, 2: 앱 복귀
            void OnFocusChanged(bool hasFocus)
            {
                if (step == 0 && !hasFocus) step = 1;
                else if (step == 1 && hasFocus) step = 2;
            }
            UnityEngine.Application.focusChanged += OnFocusChanged;
            try
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                using var intent = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS");
                using var uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse", "package:" + UnityEngine.Application.identifier);
                intent.Call<AndroidJavaObject>("setData", uri);

                CurrentActivity.Call("startActivity", intent);
#elif !UNITY_EDITOR && UNITY_IOS
                UnityEngine.Application.OpenURL("app-settings:");
#else
                step = 2;
#endif
                await AwaitableUtility.WaitUntil(() => 1 < step);
            }
            finally
            {
                UnityEngine.Application.focusChanged -= OnFocusChanged;
            }
        }
        
#if !UNITY_EDITOR && UNITY_IOS
        [DllImport("__Internal")]
        private static extern IntPtr GetiOSBuildNumber();
#endif
    }
}
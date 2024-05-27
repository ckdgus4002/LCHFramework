using System;
using System.Linq;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace LCHFramework.Utilies
{
    public static class SystemInfoUtility
    {
        public static int AndroidAPIVersion
        {
            get
            {
                if (_androidAPIVersion == null)
                {
                    _androidAPIVersion = -1;
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        var result = SystemInfo.operatingSystem;
                        var indexOf = result.IndexOf("API-", StringComparison.Ordinal);
                        if (-1 < indexOf)
                        {
                            result = result.Substring(indexOf + 4);
                            indexOf = result.IndexOf(" ", StringComparison.Ordinal);
                            if (-1 < indexOf || result.All(item => '0' <= item && item <= '9'))
                            {
                                result = result.Substring(0, -1 < indexOf ? indexOf : result.Length);
                                if (int.TryParse(result, out var value)) _androidAPIVersion = value;            
                            }
                        }
                    }
                }
                
                return (int)_androidAPIVersion;
            }
        }
        private static int? _androidAPIVersion;
        
        public static Version IOSVersion
        {
            get
            {
                if (_iOSVersion == null)
                {
#if UNITY_IOS
                    _iOSVersion = new Version(Device.systemVersion);
#endif
                }

                return _iOSVersion;
            }
        }
        private static Version _iOSVersion = null;
    }
}
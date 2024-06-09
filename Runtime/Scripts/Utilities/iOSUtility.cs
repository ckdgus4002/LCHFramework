using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace LCHFramework.Utilities
{
    public static class iOSUtility
    {
        public static Version IOSVersionOrNull
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
        private static Version _iOSVersion;
    }
}
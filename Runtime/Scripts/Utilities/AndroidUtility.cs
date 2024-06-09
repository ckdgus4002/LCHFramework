using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class AndroidUtility
    {
        public static int AndroidAPIVersion
        {
            get
            {
                if (_androidAPIVersion < 0)
                {
                    using var version = new AndroidJavaClass("android.os.Build$VERSION");
                    _androidAPIVersion = version.GetStatic<int>("SDK_INT");
                }
                
                return _androidAPIVersion;
            }
        }
        private static int _androidAPIVersion = -1;
    }
}
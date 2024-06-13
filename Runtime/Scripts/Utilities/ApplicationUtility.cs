using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class ApplicationUtility
    {
        public static bool IsEditor
        {
            get
            {
#if UNITY_2023_2_OR_NEWER
                return Application.isEditor;
#elif UNITY_EDITOR 
                return true;
#else
                return false;
#endif    
            }
        }
    }
}
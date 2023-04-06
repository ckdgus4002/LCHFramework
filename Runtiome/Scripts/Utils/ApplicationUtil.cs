namespace LCHFramework.Utils
{
    public static class ApplicationUtil
    {
        public static bool IsEditor
        {
            get
            {
                // return UnityEngine.Application.isEditor;
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif    
            }
        }
    }
}
namespace JSchool.Modules.Common.LCH
{
    public static class ApplicationUtils
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
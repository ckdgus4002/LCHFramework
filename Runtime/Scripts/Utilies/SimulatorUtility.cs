using UnityEngine;

namespace LCHFramework.Utility
{
    public static class SimulatorUtils
    {
        public static bool IsSimulator
        {
            get
            {
#if UNITY_EDITOR
                return !Application.isEditor;
#else
                return false;
#endif
            }
        }
    }
}
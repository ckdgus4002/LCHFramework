using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class SimulatorUtility
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
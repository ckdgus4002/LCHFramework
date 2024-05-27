using UnityEngine;

namespace LCHFramework.Utilies
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
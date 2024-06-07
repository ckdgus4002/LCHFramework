using UnityEngine;

namespace LCHFramework.Utilies
{
    public static class ScreenUtility
    {
        public static bool IsSizeChanged(Vector2 prevScreenSize) => !Mathf.Approximately(prevScreenSize.x, Screen.width) || !Mathf.Approximately(prevScreenSize.y, Screen.height);
    }
}

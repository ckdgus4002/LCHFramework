using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class ScreenUtility
    {
        public static Vector2 HalfSize => Size * .5f;

        public static Vector2 Size => new(Screen.width, Screen.height);
    }
}

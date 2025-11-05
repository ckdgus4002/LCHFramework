using UnityEditor;
using UnityEngine;

namespace LCHFramework
{
    public static class Screen
    {
        public static Vector2 Size => new(width, height);
        
        public static float width
        {
            get
            {
#if UNITY_EDITOR
                return Handles.GetMainGameViewSize().x;
#else
                return Screen.width;
#endif
            }
        }
        
        public static float height
        {
            get
            {
#if UNITY_EDITOR
                return Handles.GetMainGameViewSize().y;
#else
                return Screen.height;
#endif
            }
        }
    }
}
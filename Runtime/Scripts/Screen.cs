using UnityEditor;
using UnityEngine;

namespace LCHFramework
{
    public static class Screen
    {
        public static float AspectRatio => (float)width / height;

        public static Vector2Int Size => new(width, height);
        
        public static Vector2 HalfSize => new(width * 0.5f, height * 0.5f);
        
        public static int width
        {
            get
            {
#if UNITY_EDITOR
                return (int)Handles.GetMainGameViewSize().x;
#else
                return UnityEngine.Screen.width;
#endif
            }
        }
        
        public static int height
        {
            get
            {
#if UNITY_EDITOR
                return (int)Handles.GetMainGameViewSize().y;
#else
                return UnityEngine.Screen.height;
#endif
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace LCHFramework
{
    public static class Screen
    {
        public static Vector2Int Size => new(width, height);
        
        public static int width
        {
            get
            {
#if UNITY_EDITOR
                return (int)Handles.GetMainGameViewSize().x;
#else
                return Screen.width;
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
                return Screen.height;
#endif
            }
        }
    }
}
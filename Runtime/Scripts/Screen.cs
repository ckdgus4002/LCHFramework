using UnityEngine;

namespace LCHFramework
{
    public enum ScreenOrientation
    {
        Unknown,
        Portrait = 1,
        PortraitUpsideDown,
        LandscapeLeft,
        LandscapeRight,
    }
    
    public static class Screen
    {
        public static float AspectRatio => (float)width / height;
        
        public static Vector2Int Size => new(width, height);
        
        public static Vector2 HalfSize => new(width * 0.5f, height * 0.5f);

        public static int width => UnityEngine.Screen.width;
        
        public static int height => UnityEngine.Screen.height;
    }
}
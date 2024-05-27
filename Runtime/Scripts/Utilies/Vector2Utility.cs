using UnityEngine;

namespace LCHFramework.Utilies
{
    public static class Vector2Utility
    {
        public static Vector2 Half => new(0.5f, 0.5f);
        
        public static Vector2 New(float xy) => new(xy, xy);

        public static Vector2 Quarter => new(0.25f, 0.25f);
        
        public static Vector2 MaxValue => new(float.MaxValue, float.MaxValue);
        
        public static Vector2 MinValue => new(float.MinValue, float.MinValue);
    }
}
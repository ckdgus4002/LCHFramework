using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class Vector2Utility
    {
        public static Vector2 Half => New(0.5f);
        
        public static Vector2 Quarter => New(0.25f);
        
        public static Vector2 MaxValue => New(float.MaxValue);
        
        public static Vector2 MinValue => New(float.MinValue);
        
        public static Vector2 New(float xy) => new(xy, xy);
    }
}
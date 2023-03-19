using UnityEngine;

namespace LCHFramework.Utils
{
    public static class Vector2Util
    {
        public static Vector2 Divide(Vector2 v, float divisor)
        {
            v.x /= divisor;
            v.y /= divisor;
            return v;
        }
        
        public static Vector2 MaxValue => new(float.MaxValue, float.MaxValue);
        
        public static Vector2 MinValue => new(float.MinValue, float.MinValue);
        
        public static Vector2 New(float xy) => new(xy, xy);
        
        public static Vector2 Remainder(Vector2 v, Vector2 divisor) => Remainder(v, divisor.x, divisor.y);
        
        public static Vector2 Remainder(Vector2 v, float divisor) => Remainder(v, divisor, divisor);

        public static Vector2 Remainder(Vector2 v, float divisorX, float divisorY)
        {
            v.x %= divisorX;
            v.y %= divisorY;
            return v;
        }
    }
}
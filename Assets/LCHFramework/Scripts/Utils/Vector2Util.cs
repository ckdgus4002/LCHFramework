namespace LCHFramework.Utils
{
    public static class Vector2Util
    {
        public static UnityEngine.Vector2 Divide(UnityEngine.Vector2 v, float divisor)
        {
            v.x /= divisor;
            v.y /= divisor;
            return v;
        }
        
        public static UnityEngine.Vector2 MaxValue => new(float.MaxValue, float.MaxValue);
        
        public static UnityEngine.Vector2 MinValue => new(float.MinValue, float.MinValue);
        
        public static UnityEngine.Vector2 New(float xy) => new(xy, xy);
        
        public static UnityEngine.Vector2 Remainder(UnityEngine.Vector2 v, UnityEngine.Vector2 divisor) => Remainder(v, divisor.x, divisor.y);
        
        public static UnityEngine.Vector2 Remainder(UnityEngine.Vector2 v, float divisor) => Remainder(v, divisor, divisor);

        public static UnityEngine.Vector2 Remainder(UnityEngine.Vector2 v, float divisorX, float divisorY)
        {
            v.x %= divisorX;
            v.y %= divisorY;
            return v;
        }
    }
}
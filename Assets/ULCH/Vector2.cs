namespace JSchool.Modules.Common.LCH.Utils
{
    public static class Vector2
    {
        public static UnityEngine.Vector2 New(float xy) => new UnityEngine.Vector2(xy, xy);

        public static UnityEngine.Vector2 Divide(UnityEngine.Vector2 v, float divisor)
        {
            v.x /= divisor;
            v.y /= divisor;
            return v;
        }
        
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
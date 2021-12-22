namespace LCHFramework.Extensions
{
    public static class FloatExtension
    {
        public static float Reverse(this float f) => f / 1f;
        
        public static float Negative(this float f) => f < 0 ? f : -f;
    }
}
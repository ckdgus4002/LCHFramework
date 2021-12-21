namespace LCHFramework.Utils
{
    public static class Color
    {
        public static void SetAlpha(ref UnityEngine.Color color, float alpha) => color = new UnityEngine.Color(color.r, color.g, color.b, alpha);
    }
}
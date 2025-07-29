using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class ColorExtension
    {
        public static Color NewAlpha(this Color color, float a) 
            => new(color.r, color.g, color.b, a);
    }
}
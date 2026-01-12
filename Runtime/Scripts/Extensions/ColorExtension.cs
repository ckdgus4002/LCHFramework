using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class ColorExtension
    {
        public static Color SetAlpha(this Color c, float alpha) { c.a = alpha; return c; }
    }
}
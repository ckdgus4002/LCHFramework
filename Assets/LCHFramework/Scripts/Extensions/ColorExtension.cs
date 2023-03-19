using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class ColorExtension
    {
        public static Color ExNewAlpha(this Color color, float a) 
            => new(color.r, color.g, color.b, a);
        
        public static void ExSetAlpha(this ref Color color, float a)
            => color.a = a;

        /// <param name="color"></param>
        /// <param name="grayScaleFormula">
        /// 0 => BuiltInUnity
        /// 1 => Average
        /// 2 => Lightness
        /// 3 => Luminosity
        /// 4 => BT709
        /// 5 => RMY
        /// 6 => YGrayScale 
        /// </param>
        /// <note> https://hwanggoon.tistory.com/168 </note>
        public static Color ExToGrayScale(this Color color, int grayScaleFormula = 0)
        {
            var grayScale = grayScaleFormula switch
            {
                0 => color.grayscale,
                1 => (color.r + color.g + color.b) / 3f,
                2 => (Mathf.Max(color.r, color.g, color.b) + Mathf.Min(color.r, color.g, color.b)) / 2f,
                3 => color.r * 0.21f + color.g * 0.72f + color.b * 0.07f,
                4 => color.r * 0.2125f + color.g * 0.7154f + color.b * 0.0721f,
                5 => color.r * 0.5f + color.g * 0.419f + color.b * 0.081f,
                6 => color.r * 0.299f + color.g * 0.587f + color.b * 0.114f,
                _ => 0f
            };
            return new Color(grayScale, grayScale, grayScale, color.a);
        }
    }
}
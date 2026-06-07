using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Extensions
{
    public static class RawImageExtension
    {
        public static void Scale(this RawImage rawImage, float scale)
        {
            var wh = 1f / scale;
            var xy = (1f - wh) / 2f;
            rawImage.uvRect = new Rect(xy, xy, wh, wh);
        }
    }
}
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class RectTransformExtension
    {
        public static bool OverlapsAtWorld(this RectTransform rectTransform, RectTransform other)
        {
            var corner1 = new UnityEngine.Vector3[4];
            var corner2 = new UnityEngine.Vector3[4];
            rectTransform.GetWorldCorners(corner1);
            other.GetWorldCorners(corner2);

            return new Rect(corner1[0].x, corner1[0].y, corner1[2].x - corner1[0].x, corner1[2].y - corner1[0].y)
                .Overlaps(new Rect(corner2[0].x, corner2[0].y, corner2[2].x - corner2[0].x, corner2[2].y - corner2[0].y))
                ;
        }
    }
}
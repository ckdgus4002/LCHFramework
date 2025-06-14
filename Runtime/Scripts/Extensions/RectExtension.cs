using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class RectExtension
    {
        public static Rect AddX(this ref Rect rect, float add)
        {
            rect.x += add;
            return rect;
        }
        
        public static Rect SetX(this Rect rect, float x)
        {
            return new Rect(x, rect.y, rect.width, rect.height);
        }

        public static Rect SetY(this Rect rect, float y)
        {
            return new Rect(rect.x, y, rect.width, rect.height);
        }
    
        public static Rect SetWidth(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, width, rect.height);
        }
    }
}
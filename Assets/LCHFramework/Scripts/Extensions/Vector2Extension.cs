using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 AbsY(this Vector2 v) { v.y = Math.Abs(v.y); return v; }
        
        public static Vector2 AddXY(this Vector2 v, float xy) => v.AddX(xy).AddY(xy);
        
        public static Vector2 AddX(this Vector2 v, float x) { v.x += x; return v; }

        public static Vector2 AddY(this Vector2 v, float y) { v.y += y; return v; }
        
        public static Vector2 NegativeX(this Vector2 v) { v.x = v.x.Negative(); return v; }

        public static Vector2 NegativeY(this Vector2 v) { v.y = v.y.Negative(); return v; }
        
        public static Vector2 ReverseX(this Vector2 v) { v.x = v.x.Reverse(); return v; }
        
        public static Vector2 ReverseY(this Vector2 v) { v.y = v.y.Reverse(); return v; }
        
        public static Vector2 NewX(this Vector2 v, float x) => new(x, v.y);
        
        public static Vector2 NewY(this Vector2 v, float y) => new(v.x, y);
        
        public static Vector3 NewZ(this Vector2 v, float z) => new(v.x, v.y, z);
        
        public static void SetX(this ref Vector2 v, float x) => v.x = x;
        
        public static void SetY(this ref Vector2 v, float y) => v.y = y;
    }
}
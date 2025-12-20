using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 AbsX(this Vector2 v) { v.x = Math.Abs(v.x); return v; }
        
        public static Vector2 AbsY(this Vector2 v) { v.y = Math.Abs(v.y); return v; }
        
        public static Vector2 AddX(this Vector2 v, float add) { v.x += add; return v; }
        
        public static Vector2 AddY(this Vector2 v, float add) { v.y += add; return v; }
        
        public static Vector2 ModX(this Vector2 v, float divisor) { v.x %= divisor; return v; }
        
        public static Vector2 ModY(this Vector2 v, float divisor) { v.y %= divisor; return v; }
        
        public static Vector2 NegateX(this Vector2 v) { v.x = v.x.Negate(); return v; }
        
        public static Vector2 NegateY(this Vector2 v) { v.y = v.y.Negate(); return v; }
        
        public static Vector2 ReverseX(this Vector2 v) { v.x = v.x.Reverse(); return v; }
        
        public static Vector2 ReverseY(this Vector2 v) { v.y = v.y.Reverse(); return v; }
        
        public static Vector2 ScaleX(this Vector2 v, float scale) { v.x *= scale; return v; }
        
        public static Vector2 ScaleY(this Vector2 v, float scale) { v.y *= scale; return v; }
        
        public static Vector2 SetX(this Vector2 v, float x) { v.x = x; return v; }
        
        public static Vector2 SetY(this Vector2 v, float y) { v.y = y; return v; }
    }
}
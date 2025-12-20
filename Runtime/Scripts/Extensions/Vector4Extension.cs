using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector4Extension
    {
        public static Vector4 AbsX(this Vector4 v) { v.x = Math.Abs(v.x); return v; }
        
        public static Vector4 AbsY(this Vector4 v) { v.y = Math.Abs(v.y); return v; }
        
        public static Vector4 AbsZ(this Vector4 v) { v.z = Math.Abs(v.z); return v; }
        
        public static Vector4 AbsW(this Vector4 v) { v.z = Math.Abs(v.w); return v; }
        
        public static Vector4 AddX(this Vector4 v, float add) { v.x += add; return v; }
        
        public static Vector4 AddY(this Vector4 v, float add) { v.y += add; return v; }
        
        public static Vector4 AddZ(this Vector4 v, float add) { v.z += add; return v; }
        
        public static Vector4 AddW(this Vector4 v, float add) { v.w += add; return v; }
        
        public static Vector4 ModX(this Vector4 v, float divisor) { v.x %= divisor; return v; }
        
        public static Vector4 ModY(this Vector4 v, float divisor) { v.y %= divisor; return v; }
        
        public static Vector4 ModZ(this Vector4 v, float divisor) { v.z %= divisor; return v; }
        
        public static Vector4 ModW(this Vector4 v, float divisor) { v.w %= divisor; return v; }
        
        public static Vector4 NegateX(this Vector4 v) { v.x = v.x.Negate(); return v; }
        
        public static Vector4 NegateY(this Vector4 v) { v.y = v.y.Negate(); return v; }
        
        public static Vector4 NegateZ(this Vector4 v) { v.z = v.z.Negate(); return v; }
        
        public static Vector4 NegateW(this Vector4 v) { v.w = v.w.Negate(); return v; }
        
        public static Vector4 ReverseX(this Vector4 v) { v.x = v.x.Reverse(); return v; }
        
        public static Vector4 ReverseY(this Vector4 v) { v.y = v.y.Reverse(); return v; }
        
        public static Vector4 ReverseZ(this Vector4 v) { v.z = v.z.Reverse(); return v; }
        
        public static Vector4 ReverseW(this Vector4 v) { v.w = v.w.Reverse(); return v; }
        
        public static Vector4 ScaleX(this Vector4 v, float scale) { v.x *= scale; return v; }
        
        public static Vector4 ScaleY(this Vector4 v, float scale) { v.y *= scale; return v; }
        
        public static Vector4 ScaleZ(this Vector4 v, float scale) { v.z *= scale; return v; }
        
        public static Vector4 ScaleW(this Vector4 v, float scale) { v.w *= scale; return v; }
        
        public static Vector4 SetX(this Vector4 v, float x) { v.x = x; return v; }
        
        public static Vector4 SetY(this Vector4 v, float y) { v.y = y; return v; }
        
        public static Vector4 SetZ(this Vector4 v, float z) { v.z = z; return v; }
        
        public static Vector4 SetW(this Vector4 v, float w) { v.w = w; return v; }
    }
}
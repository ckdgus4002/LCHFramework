using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 AbsX(this Vector3 v) { v.x = Math.Abs(v.x); return v; }

        public static Vector3 AbsY(this Vector3 v) { v.y = Math.Abs(v.y); return v; }

        public static Vector3 AbsZ(this Vector3 v) { v.z = Math.Abs(v.z); return v; }

        public static Vector3 AddX(this Vector3 v, float add) { v.x += add; return v; }

        public static Vector3 AddY(this Vector3 v, float add) { v.y += add; return v; }

        public static Vector3 ModX(this Vector3 v, float divisor) { v.x %= divisor; return v; }

        public static Vector3 ModY(this Vector3 v, float divisor) { v.y %= divisor; return v; }
        
        public static Vector3 ModZ(this Vector3 v, float divisor) { v.z %= divisor; return v; }
        
        public static Vector3 AddZ(this Vector3 v, float add) { v.z += add; return v; }

        public static Vector3 NegateX(this Vector3 v) { v.x = v.x.Negate(); return v; }

        public static Vector3 NegateY(this Vector3 v) { v.y = v.y.Negate(); return v; }

        public static Vector3 NegateZ(this Vector3 v) { v.z = v.z.Negate(); return v; }

        public static Vector3 ReverseX(this Vector3 v) { v.x = v.x.Reverse(); return v; }

        public static Vector3 ReverseY(this Vector3 v) { v.y = v.y.Reverse(); return v; }

        public static Vector3 ReverseZ(this Vector3 v) { v.z = v.z.Reverse(); return v; }

        public static Vector3 ScaleX(this Vector3 v, float scale) { v.x *= scale; return v; }
        
        public static Vector3 ScaleY(this Vector3 v, float scale) { v.y *= scale; return v; }
        
        public static Vector3 ScaleZ(this Vector3 v, float scale) { v.z *= scale; return v; }
        
        public static Vector3 SetX(this Vector3 v, float x) { v.x = x; return v; }
        
        public static Vector3 SetY(this Vector3 v, float y) { v.y = y; return v; } 
        
        public static Vector3 SetZ(this Vector3 v, float z) { v.z = z; return v; }
    }
}
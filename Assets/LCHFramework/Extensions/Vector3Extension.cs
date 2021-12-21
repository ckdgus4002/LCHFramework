using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 AbsY(this Vector3 v) { v.y = Math.Abs(v.y); return v; }
        
        public static Vector3 AddXY(this Vector3 v, float xy) => v.AddX(xy).AddY(xy);
        
        public static Vector3 AddX(this Vector3 v, float x) { v.x += x; return v; }

        public static Vector3 AddY(this Vector3 v, float y) { v.y += y; return v; }
        
        public static Vector3 AddZ(this Vector3 v, float z) { v.z += z; return v; }

        public static Vector3 NegativeX(this Vector3 v) => v.SetX(v.x.Negative());
        
        public static Vector3 NegativeY(this Vector3 v) => v.SetY(v.y.Negative());
        
        public static Vector3 NegativeZ(this Vector3 v) => v.SetZ(v.z.Negative());
        
        public static Vector3 ReverseX(this Vector3 v) { v.x *= -1f; return v; }
        
        public static Vector3 SetX(this Vector3 v, float x) { v.x = x; return v; }
        
        public static Vector3 SetY(this Vector3 v, float y) { v.y = y; return v; }
        
        public static Vector3 SetZ(this Vector3 v, float z) { v.z = z; return v; }
    }
}
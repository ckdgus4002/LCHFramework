using System;

namespace LCHFramework.Extensions
{
    public static class Vector3Extension
    {
        public static UnityEngine.Vector3 AbsY(this UnityEngine.Vector3 v) { v.y = Math.Abs(v.y); return v; }
        
        public static UnityEngine.Vector3 AddXY(this UnityEngine.Vector3 v, float xy) => v.AddX(xy).AddY(xy);
        
        public static UnityEngine.Vector3 AddX(this UnityEngine.Vector3 v, float x) { v.x += x; return v; }

        public static UnityEngine.Vector3 AddY(this UnityEngine.Vector3 v, float y) { v.y += y; return v; }
        
        public static UnityEngine.Vector3 AddZ(this UnityEngine.Vector3 v, float z) { v.z += z; return v; }

        public static UnityEngine.Vector3 NegativeX(this UnityEngine.Vector3 v) => v.SetX(v.x.Negative());
        
        public static UnityEngine.Vector3 NegativeY(this UnityEngine.Vector3 v) => v.SetY(v.y.Negative());
        
        public static UnityEngine.Vector3 NegativeZ(this UnityEngine.Vector3 v) => v.SetZ(v.z.Negative());
        
        public static UnityEngine.Vector3 ReverseX(this UnityEngine.Vector3 v) { v.x *= -1f; return v; }
        
        public static UnityEngine.Vector3 SetX(this UnityEngine.Vector3 v, float x) { v.x = x; return v; }
        
        public static UnityEngine.Vector3 SetY(this UnityEngine.Vector3 v, float y) { v.y = y; return v; }
        
        public static UnityEngine.Vector3 SetZ(this UnityEngine.Vector3 v, float z) { v.z = z; return v; }
    }
}
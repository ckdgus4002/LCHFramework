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

        public static UnityEngine.Vector3 NegativeX(this UnityEngine.Vector3 v) { v.x = v.x.Negative(); return v; }

        public static UnityEngine.Vector3 NegativeY(this UnityEngine.Vector3 v) { v.y = v.y.Negative(); return v; }
        
        public static UnityEngine.Vector3 NegativeZ(this UnityEngine.Vector3 v) { v.z = v.z.Negative(); return v; }

        public static UnityEngine.Vector3 ReverseX(this UnityEngine.Vector3 v) { v.x = v.x.Reverse(); return v; }
        
        public static UnityEngine.Vector3 ReverseY(this UnityEngine.Vector3 v) { v.y = v.y.Reverse(); return v; }
        
        public static UnityEngine.Vector3 ReverseZ(this UnityEngine.Vector3 v) { v.z = v.z.Reverse(); return v; }

        public static UnityEngine.Vector3 NewX(this UnityEngine.Vector3 v, float x) => new(x, v.y, v.z);
        
        public static UnityEngine.Vector3 NewY(this UnityEngine.Vector3 v, float y) => new(v.x, y, v.z);
        
        public static UnityEngine.Vector3 NewZ(this UnityEngine.Vector3 v, float z) => new(v.x, v.y, z);
        
        public static void SetX(this ref UnityEngine.Vector3 v, float x) => v.x = x;
        
        public static void SetY(this ref UnityEngine.Vector3 v, float y) => v.y = y;

        public static void SetZ(this ref UnityEngine.Vector3 v, float z) => v.z = z;
    }
}
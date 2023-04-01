using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 AbsX(this Vector3 v) { v.x = Math.Abs(v.x); return v; }
        
        public static Vector3 AbsY(this Vector3 v) { v.y = Math.Abs(v.y); return v; }
        
        public static Vector3 AbsZ(this Vector3 v) { v.z = Math.Abs(v.z); return v; }
        
        public static Vector3 AddX(this Vector3 v, float add) => v.AddXYZ(new Vector3(add, 0, 0));

        public static Vector3 AddY(this Vector3 v, float add) => v.AddXYZ(new Vector3(0, add, 0));

        public static Vector3 AddZ(this Vector3 v, float add) => v.AddXYZ(new Vector3(0, 0, add));
        
        public static Vector3 AddXY(this Vector3 v, Vector2 add) => v.AddXYZ(new Vector3(add.x, add.y, 0));
        
        public static Vector3 AddXY(this Vector3 v, Vector3 add) => v.AddXYZ(new Vector3(add.x, add.y, 0));
        
        public static Vector3 AddXZ(this Vector3 v, Vector3 add) => v.AddXYZ(new Vector3(add.x, 0, add.z));
        
        public static Vector3 AddYZ(this Vector3 v, Vector3 add) => v.AddXYZ(new Vector3(0, add.y, add.z));

        private static Vector3 AddXYZ(this Vector3 v, Vector3 add) => v + add;

        public static Vector3 NegativeX(this Vector3 v) { v.x = v.x.Negative(); return v; }

        public static Vector3 NegativeY(this Vector3 v) { v.y = v.y.Negative(); return v; }
        
        public static Vector3 NegativeZ(this Vector3 v) { v.z = v.z.Negative(); return v; }
        
        public static Vector3 NewX(this Vector3 v, float x) => new(x, v.y, v.z);
        
        public static Vector3 NewY(this Vector3 v, float y) => new(v.x, y, v.z);
        
        public static Vector3 NewZ(this Vector3 v, float z) => new(v.x, v.y, z);
        
        public static Vector3 ReverseX(this Vector3 v) { v.x = v.x.Reverse(); return v; }
        
        public static Vector3 ReverseY(this Vector3 v) { v.y = v.y.Reverse(); return v; }
        
        public static Vector3 ReverseZ(this Vector3 v) { v.z = v.z.Reverse(); return v; }

        public static void SetX(this ref Vector3 v, float x) => v.x = x;
        
        public static void SetY(this ref Vector3 v, float y) => v.y = y;

        public static void SetZ(this ref Vector3 v, float z) => v.z = z;
    }
}
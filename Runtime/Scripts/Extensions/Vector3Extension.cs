using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector3Extension
    {
        public static void AbsX(this ref Vector3 v) => v.x = Math.Abs(v.x);
        
        public static void AbsY(this ref Vector3 v) => v.y = Math.Abs(v.y);
        
        public static void AbsZ(this ref Vector3 v) => v.z = Math.Abs(v.z);
        
        public static void AddX(this ref Vector3 v, float add) => v.AddXYZ(new Vector3(add, 0, 0));

        public static void AddY(this ref Vector3 v, float add) => v.AddXYZ(new Vector3(0, add, 0));

        public static void AddZ(this ref Vector3 v, float add) => v.AddXYZ(new Vector3(0, 0, add));
        
        public static void AddXY(this ref Vector3 v, Vector2 add) => v.AddXYZ(add);
        
        public static void AddXY(this ref Vector3 v, Vector3 add) => v.AddXYZ(new Vector3(add.x, add.y, 0));
        
        public static void AddXZ(this ref Vector3 v, Vector3 add) => v.AddXYZ(new Vector3(add.x, 0, add.z));
        
        public static void AddYZ(this ref Vector3 v, Vector3 add) => v.AddXYZ(new Vector3(0, add.y, add.z));

        private static void AddXYZ(this ref Vector3 v, Vector3 add) => v += add;

        public static void NegateX(this ref Vector3 v) => v.x = v.x.Negative();

        public static void NegativeY(this ref Vector3 v) => v.y = v.y.Negative();
        
        public static void NegativeZ(this ref Vector3 v) => v.z = v.z.Negative();
        
        public static Vector3 NewX(this Vector3 v, float x) => new(x, v.y, v.z);
        
        public static Vector3 NewY(this Vector3 v, float y) => new(v.x, y, v.z);
        
        public static Vector3 NewZ(this Vector3 v, float z) => new(v.x, v.y, z);
        
        public static void ReverseX(this ref Vector3 v) => v.x = v.x.Reverse();
        
        public static void ReverseY(this ref Vector3 v) => v.y = v.y.Reverse();
        
        public static void ReverseZ(this ref Vector3 v) => v.z = v.z.Reverse();

        public static void SetX(this ref Vector3 v, float x) => v.x = x;
        
        public static void SetY(this ref Vector3 v, float y) => v.y = y;

        public static void SetZ(this ref Vector3 v, float z) => v.z = z;
    }
}
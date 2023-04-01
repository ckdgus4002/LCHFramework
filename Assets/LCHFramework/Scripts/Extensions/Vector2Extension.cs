using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 AbsX(this Vector2 v) { v.x = Math.Abs(v.x); return v; }
        
        public static Vector2 AbsY(this Vector2 v) { v.y = Math.Abs(v.y); return v; }

        public static Vector3 AbsZ(this Vector2 v) => v;

        public static Vector2 AddX(this Vector2 v, float add) => v.AddXY(new Vector2(add, 0));

        public static Vector2 AddY(this Vector2 v, float add) => v.AddXY(new Vector2(0, add));

        public static Vector3 AddZ(this Vector2 v, float add) => v.AddXYZ(new Vector3(0, 0, add));

        private static Vector2 AddXY(this Vector2 v, Vector2 add) => v + add;

        public static Vector3 AddXZ(this Vector2 v, Vector3 add) => v.AddXYZ(new Vector3(add.x, 0, add.z));

        public static Vector3 AddYZ(this Vector2 v, Vector3 add) => v.AddXYZ(new Vector3(0, add.y, add.z));

        public static Vector3 AddXYZ(this Vector2 v, Vector3 add) => new(v.x + add.x, v.y + add.y, add.z);

        public static Vector2 NegativeX(this Vector2 v) { v.x = v.x.Negative(); return v; }

        public static Vector2 NegativeY(this Vector2 v) { v.y = v.y.Negative(); return v; }
        
        public static Vector2 NewX(this Vector2 v, float x) => new(x, v.y);
        
        public static Vector2 NewY(this Vector2 v, float y) => new(v.x, y);
        
        public static Vector3 NewZ(this Vector2 v, float z) => new(v.x, v.y, z);

        public static Vector2 RemainderX(this Vector2 v, float divisor) => v.RemainderXY(new Vector2(divisor, 1));
        
        public static Vector2 RemainderY(this Vector2 v, float divisor) => v.RemainderXY(new Vector2(1, divisor));

        public static Vector2 RemainderXY(this Vector2 v, float divisorXY) => v.RemainderXY(new Vector2(divisorXY, divisorXY));
        
        public static Vector2 RemainderXY(this Vector2 v, Vector2 divisor)
        {
            v.x %= divisor.x;
            v.y %= divisor.y;
            return v;
        }
        
        public static Vector2 ReverseX(this Vector2 v) { v.x = v.x.Reverse(); return v; }
        
        public static Vector2 ReverseY(this Vector2 v) { v.y = v.y.Reverse(); return v; }
        
        public static void SetX(this ref Vector2 v, float x) => v.x = x;
        
        public static void SetY(this ref Vector2 v, float y) => v.y = y;
    }
}
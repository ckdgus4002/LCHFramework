using System;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector2Extension
    {
        public static void AbsX(this ref Vector2 v) => v.x = Math.Abs(v.x);
        
        public static void AbsY(this ref Vector2 v) => v.y = Math.Abs(v.y);

        public static void AddX(this ref Vector2 v, float add) => v.AddXY(new Vector2(add, 0));

        public static void AddY(this ref Vector2 v, float add) => v.AddXY(new Vector2(0, add));

        private static void AddXY(this ref Vector2 v, Vector2 add) => v += add;

        public static void NegateX(this ref Vector2 v) => v.x = v.x.Negative();

        public static void NegateY(this ref Vector2 v) => v.y = v.y.Negative();
        
        public static Vector2 NewX(this Vector2 v, float x) => new(x, v.y);
        
        public static Vector2 NewY(this Vector2 v, float y) => new(v.x, y);
        
        public static Vector3 NewZ(this Vector2 v, float z) => new(v.x, v.y, z);

        public static void RemainderX(this ref Vector2 v, float divisor) => v.x %= divisor;

        public static void RemainderY(this ref Vector2 v, float divisor) => v.y %= divisor;

        public static void RemainderXY(this ref Vector2 v, float divisorXY) => v.RemainderXY(new Vector2(divisorXY, divisorXY));

        public static void RemainderXY(this ref Vector2 v, Vector2 divisor)
        {
            v.x %= divisor.x;
            v.y %= divisor.y;
        }
        
        public static void ReverseX(this ref Vector2 v) => v.x = v.x.Reverse();
        
        public static void ReverseY(this ref Vector2 v) => v.y = v.y.Reverse();
        
        public static void SetX(this ref Vector2 v, float x) => v.x = x;
        
        public static void SetY(this ref Vector2 v, float y) => v.y = y;
    }
}
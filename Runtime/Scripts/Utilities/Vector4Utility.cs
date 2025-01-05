using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class Vector4Utility
    {
        public static Vector4 Half => New(0.5f);
        
        public static Vector4 Quarter => New(0.25f);
        
        public static Vector4 MaxValue => New(float.MaxValue);
        
        public static Vector4 MinValue => New(float.MinValue);
        
        public static Vector4 New(float xyz) => new(xyz, xyz, xyz, xyz);
    }
}
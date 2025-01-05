using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class Vector3Utility
    {
        public static Vector3 Half => New(0.5f);

        public static Vector3 Quarter => New(0.25f);
        
        public static Vector3 MaxValue => New(float.MaxValue);
        
        public static Vector3 MinValue => New(float.MinValue);
        
        public static Vector3 New(float xyz) => new(xyz, xyz, xyz);
    }
}
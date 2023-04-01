using UnityEngine;

namespace LCHFramework.Utils
{
    public static class Vector3Util
    {
        public static Vector3 Half => new(0.5f, 0.5f, 0.5f);

        public static Vector3 New(float xyz) => new(xyz, xyz, xyz);
        
        public static Vector3 Quarter => new(0.25f, 0.25f, 0.25f);
        
        public static Vector3 MaxValue => new(float.MaxValue, float.MaxValue, float.MaxValue);
        
        public static Vector3 MinValue => new(float.MinValue, float.MinValue, float.MinValue);  
    }
}
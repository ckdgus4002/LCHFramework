namespace LCHFramework.Utils
{
    public static class Vector3Util
    {
        public static UnityEngine.Vector3 MaxValue => new(float.MaxValue, float.MaxValue, float.MaxValue);
        
        public static UnityEngine.Vector3 MinValue => new(float.MinValue, float.MinValue, float.MinValue);
        
        public static UnityEngine.Vector3 New(float xyz) => new(xyz, xyz, xyz);
    }
}
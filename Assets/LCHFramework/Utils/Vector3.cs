namespace LCHFramework.Utils
{
    public static class Vector3
    {
        public static UnityEngine.Vector3 New(float v) => new UnityEngine.Vector3(v, v, v);
        
        public static UnityEngine.Vector3 MaxValue => new UnityEngine.Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        
        public static UnityEngine.Vector3 MinValue => new UnityEngine.Vector3(float.MinValue, float.MinValue, float.MinValue);
    }
}
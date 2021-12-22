namespace LCHFramework
{
    public static class Vector3
    {
        public static UnityEngine.Vector3 New(float v) => new(v, v, v);
        
        public static UnityEngine.Vector3 MaxValue => new(float.MaxValue, float.MaxValue, float.MaxValue);
        
        public static UnityEngine.Vector3 MinValue => new(float.MinValue, float.MinValue, float.MinValue);
    }
}
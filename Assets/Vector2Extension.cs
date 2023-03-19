using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 SetX (this Vector2 v, float x) { v.x = x; return v; }
        
        public static Vector2 SetY(this Vector2 v, float y) { v.y = y; return v; }
        
        public static UnityEngine.Vector3 SetZ(this Vector2 v, float z) => new(v.x, v.y, z);
    }
}
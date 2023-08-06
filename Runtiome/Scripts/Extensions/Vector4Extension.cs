using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Vector4Extension
    {
        public static Vector4 ExNewW(this ref Vector4 v, float w)
        {
            return new Vector4(v.x, v.y, v.z, w);
        }
    }   
}
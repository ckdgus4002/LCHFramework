using System;
using UnityEngine;

namespace LCHFramework.Data
{
    [Serializable]
    public struct Vector4Bool
    {
        public static Vector4Bool False => new() { x = false, y = false, z = false, w = false };
        
        public static Vector4Bool True => new() { x = true, y = true, z = true, w = true };
        
        public static Vector4Bool OnlyX => new() { x = true, y = false, z = false, w = false };
        
        public static Vector4Bool OnlyY => new() { x = false, y = true, z = false, w = false };
        
        public static Vector4Bool OnlyZ => new() { x = false, y = false, z = true, w = false };
        
        public static Vector4Bool OnlyW => new() { x = false, y = false, z = false, w = true };
        
        public static bool operator !=(Vector4Bool a, Vector4Bool b) => !(a == b);

        public static bool operator ==(Vector4Bool a, Vector4Bool b) => (Vector4)a == (Vector4)b;

        public static Vector3 operator *(float a, Vector4Bool b) => a * (Vector4)b;
        
        public static Vector3 operator *(Vector4Bool a, float b) => (Vector4)a * b;

        public static implicit operator Vector4Bool(Vector2 value) => (Vector4)value;
        
        public static implicit operator Vector4Bool(Vector3 value) => (Vector4)value;

        public static implicit operator Vector4Bool(Vector4 value) => new()
        {
            x = 0 < value.x,
            y = 0 < value.y,
            z = 0 < value.z,
            w = 0 < value.w,
        }; 

        public static implicit operator Vector2(Vector4Bool value) => (Vector4)value;
        
        public static implicit operator Vector3(Vector4Bool value) => (Vector4)value;
        
        public static implicit operator Vector4(Vector4Bool value) => new()
        {
            x = !value.x ? 0 : 1,
            y = !value.y ? 0 : 1,
            z = !value.z ? 0 : 1,
            w = !value.w ? 0 : 1,
        };
        
        
        
        public bool x;
        public bool y;
        public bool z;
        public bool w;
        
        
        
        public override bool Equals(object obj) => obj is Vector4Bool other && Equals(other);
        
        public bool Equals(Vector4Bool other) => this == other;
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }
    }
}
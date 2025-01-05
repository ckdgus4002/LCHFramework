using System;
using UnityEngine;

namespace LCHFramework.Data
{
    [Serializable]
    public struct Vector3Bool
    {
        public static Vector3Bool False => new() { x = false, y = false, z = false };
        
        public static Vector3Bool True => new() { x = true, y = true, z = true };
        
        public static Vector3Bool OnlyX => new() { x = true, y = false, z = false };
        
        public static Vector3Bool OnlyY => new() { x = false, y = true, z = false };
        
        public static Vector3Bool OnlyZ => new() { x = false, y = false, z = true };
        
        public static bool operator !=(Vector3Bool a, Vector3Bool b) => !(a == b);

        public static bool operator ==(Vector3Bool a, Vector3Bool b) => (Vector3)a == (Vector3)b;

        public static Vector3 operator *(float a, Vector3Bool b) => a * (Vector3)b;
        
        public static Vector3 operator *(Vector3Bool a, float b) => (Vector3)a * b;

        public static implicit operator Vector3Bool(Vector2 value) => (Vector4)value;
        
        public static implicit operator Vector3Bool(Vector3 value) => (Vector4)value;

        public static implicit operator Vector3Bool(Vector4 value) => new()
        {
            x = 0 < value.x,
            y = 0 < value.y,
            z = 0 < value.z,
        }; 

        public static implicit operator Vector2(Vector3Bool value) => (Vector4)value;
        
        public static implicit operator Vector3(Vector3Bool value) => (Vector4)value;
        
        public static implicit operator Vector4(Vector3Bool value) => new()
        {
            x = !value.x ? 0 : 1,
            y = !value.y ? 0 : 1,
            z = !value.z ? 0 : 1,
        };
        
        
        
        public bool x;
        public bool y;
        public bool z;
        
        
        
        public override bool Equals(object obj) => obj is Vector3Bool other && Equals(other);
        
        public bool Equals(Vector3Bool other) => this == other;
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                return hashCode;
            }
        }
    }
}
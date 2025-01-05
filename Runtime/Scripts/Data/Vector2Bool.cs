using System;
using UnityEngine;

namespace LCHFramework.Data
{
    [Serializable]
    public struct Vector2Bool
    {
        public static Vector2Bool False => new() { x = false, y = false };
        
        public static Vector2Bool True => new() { x = true, y = true };
        
        public static Vector2Bool OnlyX => new() { x = true, y = false };
        
        public static Vector2Bool OnlyY => new() { x = false, y = true };
        
        public static bool operator !=(Vector2Bool a, Vector2Bool b) => !(a == b);

        public static bool operator ==(Vector2Bool a, Vector2Bool b) => (Vector2)a == (Vector2)b;

        public static Vector2 operator *(float a, Vector2Bool b) => a * (Vector2)b;
        
        public static Vector2 operator *(Vector2Bool a, float b) => (Vector2)a * b;

        public static implicit operator Vector2Bool(Vector2 value) => (Vector4)value;
        
        public static implicit operator Vector2Bool(Vector3 value) => (Vector4)value;

        public static implicit operator Vector2Bool(Vector4 value) => new()
        {
            x = 0 < value.x,
            y = 0 < value.y,
        }; 

        public static implicit operator Vector2(Vector2Bool value) => (Vector4)value;
        
        public static implicit operator Vector3(Vector2Bool value) => (Vector4)value;
        
        public static implicit operator Vector4(Vector2Bool value) => new()
        {
            x = !value.x ? 0 : 1,
            y = !value.y ? 0 : 1,
        };
        
        
        
        public bool x;
        public bool y;
        
        
        
        public override bool Equals(object obj) => obj is Vector2Bool other && Equals(other);

        public bool Equals(Vector2Bool other) => this == other;
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                return hashCode;
            }
        }
    }
}
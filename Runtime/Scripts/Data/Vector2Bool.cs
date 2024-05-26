using System;
using UnityEngine;

namespace LCHFramework.Data
{
    [Serializable]
    public struct Vector2Bool
    {
        public Vector2Bool(bool value)
        {
            x = y = value;   
        }
        
        public Vector2Bool(bool x, bool y)
        {
            this.x = x;
            this.y = y;
        }
        
        
        
        public static Vector2Bool False => new(false);
        
        public static Vector2Bool True => new(true);
        
        public static Vector2Bool OnlyX => new(true, false);
        
        public static Vector2Bool OnlyY => new(false, true);
        
        public static bool operator !=(Vector2Bool a, Vector2Bool b) => !(a == b);

        public static bool operator ==(Vector2Bool a, Vector2Bool b) => (Vector2)a == (Vector2)b;

        public static Vector2 operator *(float a, Vector2Bool b) => a * (Vector2)b;
        
        public static Vector2 operator *(Vector2Bool a, float b) => (Vector2)a * b;

        public static implicit operator Vector2Bool(Vector3 value) => (Vector2)value;
        
        public static implicit operator Vector2Bool(Vector2 value) => new()
        {
            x = Convert.ToBoolean(value.x),
            y = Convert.ToBoolean(value.y),
        };

        public static implicit operator Vector3(Vector2Bool value) => (Vector2)value;
        
        public static implicit operator Vector2(Vector2Bool value) => new()
        {
            x = Convert.ToSingle(value.x),
            y = Convert.ToSingle(value.y),
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
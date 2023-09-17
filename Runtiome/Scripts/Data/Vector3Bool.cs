using System;
using UnityEngine;

namespace LCHFramework.Data
{
    [Serializable]
    public struct Vector3Bool
    {
        public Vector3Bool(bool value)
        {
            x = y = z = value;
        }
        
        public Vector3Bool(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        
        
        public static Vector3Bool False => new(false);
        
        public static Vector3Bool True => new(true);
        
        public static Vector3Bool OnlyX => new(true, false, false);
        
        public static Vector3Bool OnlyY => new(false, true, false);
        
        public static Vector3Bool OnlyZ => new(false, false, true);
        
        public static bool operator !=(Vector3Bool a, Vector3Bool b) => !(a == b);

        public static bool operator ==(Vector3Bool a, Vector3Bool b) => (Vector3)a == (Vector3)b;

        public static Vector3 operator *(float a, Vector3Bool b) => a * (Vector3)b;
        
        public static Vector3 operator *(Vector3Bool a, float b) => (Vector3)a * b;

        public static implicit operator Vector3Bool(Vector2 value) => (Vector3)value;
        
        public static implicit operator Vector3Bool(Vector3 value) => new()
        {
            x = Convert.ToBoolean(value.x),
            y = Convert.ToBoolean(value.y),
            z = Convert.ToBoolean(value.z),
        };

        public static implicit operator Vector2(Vector3Bool value) => (Vector3)value;
        
        public static implicit operator Vector3(Vector3Bool value) => new()
        {
            x = Convert.ToSingle(value.x),
            y = Convert.ToSingle(value.y),
            z = Convert.ToSingle(value.z),
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
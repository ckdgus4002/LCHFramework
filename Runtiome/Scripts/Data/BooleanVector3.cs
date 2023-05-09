using System;
using UnityEngine;

namespace LCHFramework.Data
{
    [Serializable]
    public struct BooleanVector3
    {
        public BooleanVector3(bool value)
        {
            x = y = z = value;
        }
        
        public BooleanVector3(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        
        
        public static BooleanVector3 False => new BooleanVector3(false, false, false);
        
        public static BooleanVector3 OnlyX => new BooleanVector3(true, false, false);
        
        public static BooleanVector3 OnlyY => new BooleanVector3(false, true, false);
        
        public static BooleanVector3 OnlyZ => new BooleanVector3(false, false, true);
        
        
        
        
        public bool x;
        public bool y;
        public bool z;
        
        
        
        public override bool Equals(object obj) => obj is BooleanVector3 other && Equals(other);

        public bool Equals(BooleanVector3 other) => this == other;
        
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
        
        
                
        public static bool operator !=(BooleanVector3 a, BooleanVector3 b) => !(a == b);

        public static bool operator ==(BooleanVector3 a, BooleanVector3 b) => (Vector3)a == (Vector3)b;

        public static Vector3 operator *(float a, BooleanVector3 b) => a * (Vector3)b;
        
        public static Vector3 operator *(BooleanVector3 a, float b) => (Vector3)a * b;
        
        public static implicit operator BooleanVector3(Vector3 value) => new BooleanVector3
        {
            x = Convert.ToBoolean(value.x),
            y = Convert.ToBoolean(value.y),
            z = Convert.ToBoolean(value.z),
        };

        public static implicit operator Vector2(BooleanVector3 value) => (Vector3)value;
        
        public static implicit operator Vector3(BooleanVector3 value) => new Vector3
        {
            x = Convert.ToSingle(value.x),
            y = Convert.ToSingle(value.y),
            z = Convert.ToSingle(value.z),
        };
    }
}
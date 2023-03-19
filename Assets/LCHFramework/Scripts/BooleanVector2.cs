using System;
using UnityEngine;

namespace LCHFramework
{
    [Serializable]
    public struct BooleanVector2
    {
        public BooleanVector2(bool value)
        {
            x = y = value;   
        }
        
        
        
        public bool x;
        public bool y;
        
        
        
        public static implicit operator Vector2(BooleanVector2 value) => new Vector2
        {
            x = Convert.ToSingle(value.x),
            y = Convert.ToSingle(value.y),
        };
    }
}
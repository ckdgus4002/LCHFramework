using System;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StringEnumAttribute : PropertyAttribute
    {
        public StringEnumAttribute(params string[] strings) 
        {
            Strings = strings;
        }
        
        
        
        public string[] Strings { get; private set; }
    }   
}
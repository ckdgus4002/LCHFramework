using System;
using UnityEngine;

namespace LCHFramework.Attributes
{
    public class StringInListAttribute : PropertyAttribute
    {
        public StringInListAttribute(params string [] list) 
        {
            List = list;
        }
    
        public StringInListAttribute(Type type, string methodName) 
        {
            var method = type.GetMethod(methodName);
            if (method == null) throw new NullReferenceException($"NO SUCH METHOD {methodName} FOR {type}.");
            else List = method.Invoke (null, null) as string[];
        }
        
        
        
        public string[] List { get; }
    }   
}
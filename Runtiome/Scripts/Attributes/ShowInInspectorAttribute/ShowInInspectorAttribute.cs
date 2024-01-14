using System;
using System.Reflection;
using UnityEngine;

namespace ShowInInspector
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ShowInInspectorAttribute : PropertyAttribute
    {
        public ShowInInspectorAttribute()
        {
        }

        public ShowInInspectorAttribute(string labelName)
        {
            this.labelName = labelName;
        }
        
        
        
        public string labelName;
        public MethodInfo methodInfo;
    }   
}
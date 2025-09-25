using System;
using System.Reflection;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public ButtonAttribute()
        {
            labelName = "";
        }

        public ButtonAttribute(string labelName)
        {
            this.labelName = labelName;
        }
        
        
        
        public string labelName;
        public MethodInfo methodInfo;
    }   
}
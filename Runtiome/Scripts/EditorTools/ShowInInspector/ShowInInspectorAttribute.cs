using System;
using System.Reflection;
using UnityEngine;

namespace LCHFramework.EditorTools.ShowInInspector
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ShowInInspectorAttribute : PropertyAttribute
    {
        public string labelName;
        public string methodName;
        public MethodInfo methodInfo;

        

        public MethodInfo GetMethod(object obj) => obj.GetType().GetMethod(methodName);
    }   
}
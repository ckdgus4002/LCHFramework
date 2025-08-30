using System;
using System.Reflection;
using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnableInInspectorAttribute : PropertyAttribute, IInInspectorAttribute
    {
        public EnableInInspectorAttribute(string targetName, ComparisonOperator comparisonOperator = ComparisonOperator.Equals, object comparisonValue = null)
        {
            TargetName = targetName;
            ComparisonOperator = comparisonOperator;
            ComparisonValue = comparisonValue;
            Result = null;
        }
        
        public EnableInInspectorAttribute(bool result)
        {
            TargetName = "";
            ComparisonOperator = ComparisonOperator.Equals;
            ComparisonValue = null;
            Result = result;
        }
        
        
        
        public string TargetName { get; }
        public ComparisonOperator ComparisonOperator { get; }
        public object ComparisonValue { get; }
        public bool? Result { get; set; }
        public FieldInfo FieldInfo { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}
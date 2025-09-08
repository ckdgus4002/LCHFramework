using System;
using System.Reflection;
using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnableInInspectorAttribute : PropertyAttribute, IInInspectorAttribute
    {
        public EnableInInspectorAttribute(string targetName)
        {
            TargetName = targetName;
            NeedInitializeComparison = true;
            ComparisonOperator = ComparisonOperator.Equals;
            ComparisonValue = null;
            Result = null;
        }
        
        public EnableInInspectorAttribute(string targetName, ComparisonOperator comparisonOperator, object comparisonValue)
        {
            TargetName = targetName;
            NeedInitializeComparison = false;
            ComparisonOperator = comparisonOperator;
            ComparisonValue = comparisonValue;
            Result = null;
        }
        
        public EnableInInspectorAttribute(bool result)
        {
            TargetName = "";
            NeedInitializeComparison = false;
            ComparisonOperator = ComparisonOperator.Equals;
            ComparisonValue = null;
            Result = result;
        }
        
        
        
        public string TargetName { get; }
        public bool NeedInitializeComparison { get; }
        public ComparisonOperator ComparisonOperator { get; set; }
        public object ComparisonValue { get; set; }
        public bool? Result { get; set; }
        public FieldInfo FieldInfo { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}
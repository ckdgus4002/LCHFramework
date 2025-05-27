using System;
using System.Reflection;
using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnableIfAttribute : PropertyAttribute, IIfAttribute
    {
        public EnableIfAttribute(string targetName, ComparisonOperator comparisonOperator = ComparisonOperator.Equals, object comparisonValue = null)
        {
            TargetName = targetName;
            ComparisonOperator = comparisonOperator;
            ComparisonValue = comparisonValue;
            Force = null;
        }
        
        public EnableIfAttribute(bool force)
        {
            TargetName = "";
            ComparisonOperator = ComparisonOperator.Equals;
            ComparisonValue = null;
            Force = force;
        }
        
        
        
        public string TargetName { get; }
        public ComparisonOperator ComparisonOperator { get; }
        public object ComparisonValue { get; }
        public bool? Force { get; }
        public FieldInfo FieldInfo { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }
}
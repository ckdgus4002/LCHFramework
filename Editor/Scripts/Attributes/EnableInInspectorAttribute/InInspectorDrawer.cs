using System;
using LCHFramework.Data;
using LCHFramework.Utilities;
using UnityEditor;

namespace LCHFramework.Attributes
{
    public class InInspectorDrawer : PropertyDrawer
    {
        private const bool DefaultResult = true;
        
        
        
        protected bool GetInInspectorAttributeResult(IInInspectorAttribute inInspectorAttribute, SerializedProperty property)
        {
            if (inInspectorAttribute.Result != null) return (bool)inInspectorAttribute.Result;
            
            
            inInspectorAttribute.FieldInfo ??= property.serializedObject.targetObject.GetType().GetField(inInspectorAttribute.TargetName, TypeUtility.MaxBindingFlags);
            inInspectorAttribute.PropertyInfo ??= property.serializedObject.targetObject.GetType().GetProperty(inInspectorAttribute.TargetName, TypeUtility.MaxBindingFlags);
            inInspectorAttribute.MethodInfo ??= property.serializedObject.targetObject.GetType().GetMethod(inInspectorAttribute.TargetName, TypeUtility.MaxBindingFlags);
            if (inInspectorAttribute.FieldInfo == null && inInspectorAttribute.PropertyInfo == null && inInspectorAttribute.MethodInfo == null) return DefaultResult;
            
            
            var value = inInspectorAttribute.FieldInfo != null ? inInspectorAttribute.FieldInfo.GetValue(property.serializedObject.targetObject) 
                : inInspectorAttribute.PropertyInfo != null ? inInspectorAttribute.PropertyInfo.GetValue(property.serializedObject.targetObject)
                : inInspectorAttribute.MethodInfo != null ? inInspectorAttribute.MethodInfo.Invoke(property.serializedObject.targetObject, null)
                : null;
            inInspectorAttribute.ComparisonOperator = !inInspectorAttribute.NeedInitializeComparison ? inInspectorAttribute.ComparisonOperator
                : !value!.GetType().IsValueType ? ComparisonOperator.NotEquals
                : ComparisonOperator.Equals;
            inInspectorAttribute.ComparisonValue = !inInspectorAttribute.NeedInitializeComparison ? inInspectorAttribute.ComparisonValue
                : value is bool ? true
                : null;
            
            
            switch (inInspectorAttribute.ComparisonValue)
            {
                case bool:
                {
                    if (inInspectorAttribute.ComparisonOperator != ComparisonOperator.Equals && inInspectorAttribute.ComparisonOperator != ComparisonOperator.NotEquals) return DefaultResult;

                    var a = Convert.ToBoolean(value);
                    var b = Convert.ToBoolean(inInspectorAttribute.ComparisonValue);
                    return inInspectorAttribute.ComparisonOperator switch
                    {
                        ComparisonOperator.Equals => a == b,
                        ComparisonOperator.NotEquals => a != b,
                        _ => DefaultResult
                    };
                }
                case int or float or Enum or double or decimal or short or long or ushort or uint or ulong or byte or sbyte:
                {
                    var a = Convert.ToDecimal(value);
                    var b = Convert.ToDecimal(inInspectorAttribute.ComparisonValue);
                    return inInspectorAttribute.ComparisonOperator switch
                    {
                        ComparisonOperator.Equals => a == b,
                        ComparisonOperator.NotEquals => a != b,
                        ComparisonOperator.LessThan => a < b,
                        ComparisonOperator.LessThanEquals => a <= b,
                        ComparisonOperator.GreaterThan => b < a,
                        ComparisonOperator.GreaterThanEquals => b <= a,
                        _ => DefaultResult
                    };
                }
                case string:
                {
                    var a = Convert.ToString(value);
                    var b = Convert.ToString(inInspectorAttribute.ComparisonValue);
                    return inInspectorAttribute.ComparisonOperator switch
                    {
                        ComparisonOperator.Equals => a == b,
                        ComparisonOperator.NotEquals => a != b,
                        ComparisonOperator.LessThan => string.Compare(a, b, StringComparison.Ordinal) < 0,
                        ComparisonOperator.LessThanEquals => string.Compare(a, b, StringComparison.Ordinal) <= 0,
                        ComparisonOperator.GreaterThan => 0 < string.Compare(a, b, StringComparison.Ordinal),
                        ComparisonOperator.GreaterThanEquals => 0 <= string.Compare(a, b, StringComparison.Ordinal),
                        _ => DefaultResult,
                    };
                }
                default:
                {
                    if (inInspectorAttribute.ComparisonOperator != ComparisonOperator.Equals && inInspectorAttribute.ComparisonOperator != ComparisonOperator.NotEquals) return DefaultResult;

                    var a = value!.ToString() == "null" ? null : value;
                    var b = inInspectorAttribute.ComparisonValue;
                    return inInspectorAttribute.ComparisonOperator switch
                    {
                        ComparisonOperator.Equals => a == b,
                        ComparisonOperator.NotEquals => a != b,
                        _ => DefaultResult
                    };
                }
            }
        }
    }
}
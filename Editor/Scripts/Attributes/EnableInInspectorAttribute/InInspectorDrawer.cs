using System;
using LCHFramework.Data;
using LCHFramework.Utilities;
using UnityEditor;
using Object = UnityEngine.Object;

namespace LCHFramework.Attributes
{
    public class InInspectorDrawer : PropertyDrawer
    {
        private const bool DefaultResult = true;
        
        private object lastValue;
        private string lastValueStringIfIed;
        private string targetValueStringIfIed;
        private decimal targetValueDecimalIed;
        
        
        
        protected bool GetIfAttributeResult(IInInspectorAttribute inInspectorAttribute, SerializedProperty property)
        {
            if (inInspectorAttribute.Result != null) return (bool)inInspectorAttribute.Result;
            
            inInspectorAttribute.FieldInfo ??= property.serializedObject.targetObject.GetType().GetField(inInspectorAttribute.TargetName, TypeUtility.MaxBindingFlags);
            inInspectorAttribute.PropertyInfo ??= property.serializedObject.targetObject.GetType().GetProperty(inInspectorAttribute.TargetName, TypeUtility.MaxBindingFlags);
            inInspectorAttribute.MethodInfo ??= property.serializedObject.targetObject.GetType().GetMethod(inInspectorAttribute.TargetName, TypeUtility.MaxBindingFlags);
            if (inInspectorAttribute.FieldInfo == null && inInspectorAttribute.PropertyInfo == null && inInspectorAttribute.MethodInfo == null) return DefaultResult;

            bool result;
            var comparisonValueIsNumber = inInspectorAttribute.ComparisonValue is int || inInspectorAttribute.ComparisonValue is float || inInspectorAttribute.ComparisonValue is Enum || inInspectorAttribute.ComparisonValue is double
                                          || inInspectorAttribute.ComparisonValue is decimal || inInspectorAttribute.ComparisonValue is short || inInspectorAttribute.ComparisonValue is long || inInspectorAttribute.ComparisonValue is ushort
                                          || inInspectorAttribute.ComparisonValue is uint || inInspectorAttribute.ComparisonValue is ulong || inInspectorAttribute.ComparisonValue is byte || inInspectorAttribute.ComparisonValue is sbyte;
            if (inInspectorAttribute.ComparisonValue == null)
            {
                targetValueStringIfIed = string.Empty;
                result = inInspectorAttribute.ComparisonOperator != ComparisonOperator.Equals && inInspectorAttribute.ComparisonOperator != ComparisonOperator.NotEquals || GetHeightAllOpsString(inInspectorAttribute, property);
            }
            else if (inInspectorAttribute.ComparisonValue is bool)
            {
                targetValueStringIfIed = inInspectorAttribute.ComparisonValue.ToString();
                result = inInspectorAttribute.ComparisonOperator != ComparisonOperator.Equals && inInspectorAttribute.ComparisonOperator != ComparisonOperator.NotEquals || GetHeightAllOpsString(inInspectorAttribute, property);
            }
            else if (comparisonValueIsNumber)
            {
                targetValueDecimalIed = Convert.ToDecimal(inInspectorAttribute.ComparisonValue);
                result = GetHeightAllOpsScalar(inInspectorAttribute, property);
            }
            else
            {
                targetValueStringIfIed = inInspectorAttribute.ComparisonValue.ToString();
                result = GetHeightAllOpsString(inInspectorAttribute, property);
            }

            // Force the next update.
            var newValue = GetNewValue(inInspectorAttribute, property);

            if (lastValue == newValue)
                lastValue = true;

            // return (bool)(ifAttribute.Result = result);
            return result;
        }

        private bool GetHeightAllOpsScalar(IInInspectorAttribute inInspectorAttribute, SerializedProperty property)
        {
            if (inInspectorAttribute.FieldInfo == null && inInspectorAttribute.PropertyInfo == null && inInspectorAttribute.MethodInfo == null) return DefaultResult;
            
            var newValue = GetNewValue(inInspectorAttribute, property);
            if (!newValue.Equals(lastValue))
            {
                lastValue = newValue;
            }
            var value = Convert.ToDecimal(newValue);
            
            return inInspectorAttribute.ComparisonOperator switch
            {
                ComparisonOperator.Equals => value == targetValueDecimalIed,
                ComparisonOperator.NotEquals => value != targetValueDecimalIed,
                ComparisonOperator.LessThan => value < targetValueDecimalIed,
                ComparisonOperator.LessThanEquals => value <= targetValueDecimalIed,
                ComparisonOperator.GreaterThan => targetValueDecimalIed < value,
                _ => targetValueDecimalIed <= value,
            };
        }
        
        private bool GetHeightAllOpsString(IInInspectorAttribute inInspectorAttribute, SerializedProperty property)
        {
            if (inInspectorAttribute.FieldInfo == null && inInspectorAttribute.PropertyInfo == null && inInspectorAttribute.MethodInfo == null) return DefaultResult;
            
            var newValue = GetNewValue(inInspectorAttribute, property);
            if (lastValue != newValue)
            {
                lastValue = newValue;
                lastValueStringIfIed = lastValue != null && (lastValue is not Object o || o.ToString() != "null") // Unity Object is not referenced as real null, it is fake. Don't trust them.
                    ? lastValue.ToString()
                    : string.Empty;
            }

            return inInspectorAttribute.ComparisonOperator switch
            {
                ComparisonOperator.Equals => lastValueStringIfIed.Equals(targetValueStringIfIed),
                ComparisonOperator.NotEquals => !lastValueStringIfIed.Equals(targetValueStringIfIed),
                ComparisonOperator.LessThan => string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal) < 0,
                ComparisonOperator.LessThanEquals => string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal) <= 0,
                ComparisonOperator.GreaterThan => 0 < string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal),
                _ => 0 <= string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal),
            };
        }

        private object GetNewValue(IInInspectorAttribute inInspectorAttribute, SerializedProperty property)
            => inInspectorAttribute.FieldInfo != null ? inInspectorAttribute.FieldInfo.GetValue(property.serializedObject.targetObject)
            : inInspectorAttribute.PropertyInfo != null ? inInspectorAttribute.PropertyInfo.GetValue(property.serializedObject.targetObject)
            : inInspectorAttribute.MethodInfo.Invoke(property.serializedObject.targetObject, null);
    }
}
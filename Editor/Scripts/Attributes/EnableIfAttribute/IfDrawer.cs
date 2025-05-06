using System;
using System.Reflection;
using LCHFramework.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.LightTransport;
using Object = UnityEngine.Object;

namespace LCHFramework.Attributes
{
    public class IfDrawer : PropertyDrawer
    {
	    private object lastValue;
	    private string lastValueStringIfIed;
	    private string targetValueStringIfIed;
	    private decimal targetValueDecimalIed;
		
		
	    
        protected bool GetIfAttributeResult(IIfAttribute ifAttribute, SerializedProperty property)
        {
	        if (ifAttribute.Force != null) return (bool)ifAttribute.Force;
	        
			ifAttribute.FieldInfo ??= fieldInfo.DeclaringType!.GetField(ifAttribute.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (ifAttribute.FieldInfo == null) return true;

			var result = true;
			var comparisonValueIsNumber = ifAttribute.ComparisonValue is int || ifAttribute.ComparisonValue is float || ifAttribute.ComparisonValue is Enum || ifAttribute.ComparisonValue is double
			                              || ifAttribute.ComparisonValue is decimal || ifAttribute.ComparisonValue is short || ifAttribute.ComparisonValue is long || ifAttribute.ComparisonValue is ushort
			                              || ifAttribute.ComparisonValue is uint || ifAttribute.ComparisonValue is ulong || ifAttribute.ComparisonValue is byte || ifAttribute.ComparisonValue is sbyte;
			if (ifAttribute.ComparisonValue == null)
			{
				targetValueStringIfIed = string.Empty;
				result = ifAttribute.ComparisonOperator != ComparisonOperator.Equals && ifAttribute.ComparisonOperator != ComparisonOperator.NotEquals || GetHeightAllOpsString(ifAttribute, property);
			}
			else if (ifAttribute.ComparisonValue is bool)
			{
				targetValueStringIfIed = ifAttribute.ComparisonValue.ToString();
				result = ifAttribute.ComparisonOperator != ComparisonOperator.Equals && ifAttribute.ComparisonOperator != ComparisonOperator.NotEquals || GetHeightAllOpsString(ifAttribute, property);
			}
			else if (comparisonValueIsNumber)
			{
				targetValueDecimalIed = Convert.ToDecimal(ifAttribute.ComparisonValue);
				result = GetHeightAllOpsScalar(ifAttribute, property);
			}
			else
			{
				targetValueStringIfIed = ifAttribute.ComparisonValue.ToString();
				result = GetHeightAllOpsString(ifAttribute, property);
			}

			// Force the next update.
			var	newValue = ifAttribute.FieldInfo.GetValue(property.serializedObject.targetObject);

			if (lastValue == newValue)
				lastValue = true;

			return result;
		}

		private bool GetHeightAllOpsString(IIfAttribute ifAttribute, SerializedProperty property)
		{
			var	newValue = ifAttribute.FieldInfo.GetValue(property.serializedObject.targetObject);
			if (lastValue != newValue)
			{
				lastValue = newValue;
				lastValueStringIfIed = lastValue != null && (lastValue is not Object o || o.ToString() != "null") // Unity Object is not referenced as real null, it is fake. Don't trust them.
					? lastValue.ToString()
					: string.Empty;
			}

			return ifAttribute.ComparisonOperator switch
			{
				ComparisonOperator.Equals => lastValueStringIfIed.Equals(targetValueStringIfIed),
				ComparisonOperator.NotEquals => !lastValueStringIfIed.Equals(targetValueStringIfIed),
				ComparisonOperator.LessThan => string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal) < 0,
				ComparisonOperator.LessThanEquals => string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal) <= 0,
				ComparisonOperator.GreaterThan => 0 < string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal),
				_ => 0 <= string.Compare(lastValueStringIfIed, targetValueStringIfIed, StringComparison.Ordinal),
			};
		}

		private bool GetHeightAllOpsScalar(IIfAttribute ifAttribute, SerializedProperty property)
		{
			var newValue = ifAttribute.FieldInfo.GetValue(property.serializedObject.targetObject);
			if (!newValue.Equals(lastValue)) lastValue = newValue;
			
			var	value = Convert.ToDecimal(newValue);
			return ifAttribute.ComparisonOperator switch
			{
				ComparisonOperator.Equals => value == targetValueDecimalIed,
				ComparisonOperator.NotEquals => value != targetValueDecimalIed,
				ComparisonOperator.LessThan => value < targetValueDecimalIed,
				ComparisonOperator.LessThanEquals => value <= targetValueDecimalIed,
				ComparisonOperator.GreaterThan => targetValueDecimalIed < value,
				_ => targetValueDecimalIed <= value,
			};
		}
    }
}
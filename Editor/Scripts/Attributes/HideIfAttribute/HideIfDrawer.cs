using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Attributes
{
	[CustomPropertyDrawer(typeof(HideIfAttribute))]
	public sealed class HideIfDrawer : PropertyDrawer
	{
		private HideIfRenderer renderer;
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (renderer == null)
				renderer = new HideIfRenderer("HideIf", this, base.GetPropertyHeight, false);

			return renderer.GetPropertyHeight(property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			renderer.OnGUI(position, property, label);
		}
	}

	public sealed class HideIfRenderer
	{
		private const float	EmptyHeight = -2F;

		private readonly string name;
		private readonly Func<SerializedProperty, GUIContent, float>	getPropertyHeight;
		private readonly PropertyDrawer drawer;
		private readonly bool normalBooleanValue;

		private string errorAttribute;
		private FieldInfo conditionField;
		private string fieldName;
		private Op @operator;
		private MultiOp multiOperator;
		private object[] values;
		
		private object lastValue;
		private string lastValueStringIfIed;
		private string[] targetValueStringIfIed;
		private decimal[] targetValueDecimalIed;

		private bool conditionResult;
		private bool invalidHeight = true;
		private float cachedHeight;

		private Func<SerializedProperty, GUIContent, float>	propertyHeight;

		public HideIfRenderer(string name, PropertyDrawer drawer, Func<SerializedProperty, GUIContent, float> getPropertyHeight, bool normalBooleanValue)
		{
			this.name = name;
			this.drawer = drawer;
			this.getPropertyHeight = getPropertyHeight;
			this.normalBooleanValue = normalBooleanValue;
		}

		public float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (fieldName == null)
				InitializeDrawer(property);

			if (errorAttribute != null)
				return 16F;
			if (conditionField == null)
				return getPropertyHeight(property, label);

			return propertyHeight(property, label);
		}
		
		public void	OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (errorAttribute != null)
			{
				var restore = GUI.contentColor;
				GUI.contentColor = Color.black;
				EditorGUI.LabelField(position, label.text, errorAttribute);
				GUI.contentColor = restore;
			}
			else if (conditionField == null || conditionResult == normalBooleanValue)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(position, property, label, property.isExpanded);
				if (EditorGUI.EndChangeCheck())
					invalidHeight = true;
			}
		}

		private void InitializeDrawer(SerializedProperty property)
		{
			if (drawer.attribute is HideIfAttribute hideIfAttr)
			{
				fieldName = hideIfAttr.fieldName;
				@operator = hideIfAttr.@operator;
				multiOperator = hideIfAttr.multiOperator;
				values = hideIfAttr.values;
			}
			else
				errorAttribute = "HideIfAttribute is required by field " + name + ".";

			conditionField = drawer.fieldInfo.DeclaringType!.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (conditionField == null)
			{
				errorAttribute = name + " is requiring field \"" + fieldName + "\".";
				return;
			}
			
			if (@operator != Op.None)
			{
				if (values[0] == null)
				{
					targetValueStringIfIed = new[] { string.Empty };
					propertyHeight = GetHeightAllOpsString;

					if (@operator != Op.Equals && @operator != Op.Diff)
						errorAttribute = name + " is requiring a null value whereas its operator is \"" + @operator + "\" which is impossible.";
				}
				else if (values[0] is bool)
				{
					targetValueStringIfIed = new[] { values[0].ToString() };
					propertyHeight = GetHeightAllOpsString;

					if (@operator != Op.Equals && @operator != Op.Diff)
						errorAttribute = name + " is requiring a boolean whereas its operator is \"" + @operator + "\" which is impossible.";
				}
				else if (values[0] is int || values[0] is float || values[0] is Enum || values[0] is double
				         || values[0] is decimal || values[0] is short || values[0] is long || values[0] is ushort
				         || values[0] is uint || values[0] is ulong || values[0] is byte || values[0] is sbyte)
				{
					targetValueDecimalIed = new[] { Convert.ToDecimal(values[0]) };
					propertyHeight = GetHeightAllOpsScalar;
				}
				else
				{
					targetValueStringIfIed = new[] { values[0].ToString() };
					propertyHeight = GetHeightAllOpsString;
				}
			}
			else if (multiOperator != MultiOp.None)
			{
				var checkUseOfNonScalarValue = values.Any(t => t is null or string or bool);
				if (checkUseOfNonScalarValue)
				{
					targetValueStringIfIed = new string[values.Length];
					for (var i = 0; i < values.Length; i++)
						targetValueStringIfIed[i] = values[i] != null ? values[i].ToString() : string.Empty;

					propertyHeight = GetHeightMultiOpsString;
				}
				else
				{
					targetValueDecimalIed = new decimal[values.Length];
					for (var i = 0; i < values.Length; i++)
						targetValueDecimalIed[i] = Convert.ToDecimal(values[i]);

					propertyHeight = GetHeightMultiOpsScalar;
				}
			}

			// Force the next update.
			var	newValue = conditionField.GetValue(property.serializedObject.targetObject);

			if (lastValue == newValue)
				lastValue = true;
		}

		private float GetHeightAllOpsString(SerializedProperty property, GUIContent label)
		{
			var	newValue = conditionField.GetValue(property.serializedObject.targetObject);

			if (lastValue != newValue)
			{
				lastValue = newValue;
				lastValueStringIfIed = lastValue != null
				                       && (!typeof(Object).IsAssignableFrom(lastValue.GetType()) || (lastValue as Object)?.ToString() != "null") // Unity Object is not referenced as real null, it is fake. Don't trust them.
					? lastValue.ToString()
					: string.Empty;

				conditionResult = @operator switch
				{
					Op.Equals => lastValueStringIfIed.Equals(targetValueStringIfIed[0]),
					Op.Diff => lastValueStringIfIed.Equals(targetValueStringIfIed[0]) == false,
					Op.Sup => 0 < string.Compare(lastValueStringIfIed, targetValueStringIfIed[0], StringComparison.Ordinal),
					Op.Inf => string.Compare(lastValueStringIfIed, targetValueStringIfIed[0], StringComparison.Ordinal) < 0,
					Op.SupEquals => 0 <= string.Compare(lastValueStringIfIed, targetValueStringIfIed[0], StringComparison.Ordinal),
					Op.InfEquals => string.Compare(lastValueStringIfIed, targetValueStringIfIed[0], StringComparison.Ordinal) <= 0,
					_ => conditionResult
				};
			}

			return CalculateHeight(property, label);
		}

		private float GetHeightAllOpsScalar(SerializedProperty property, GUIContent label)
		{
			var newValue = conditionField.GetValue(property.serializedObject.targetObject);

			if (newValue.Equals(lastValue) == false)
			{
				lastValue = newValue;

				try
				{
					var	value = Convert.ToDecimal(newValue);

					conditionResult = @operator switch
					{
						Op.Equals => value == targetValueDecimalIed[0],
						Op.Diff => conditionResult = value != targetValueDecimalIed[0],
						Op.Sup => conditionResult = targetValueDecimalIed[0] < value,
						Op.Inf => conditionResult = value < targetValueDecimalIed[0],
						Op.SupEquals => conditionResult = targetValueDecimalIed[0] <= value,
						Op.InfEquals => conditionResult = value <= targetValueDecimalIed[0],
						_ => conditionResult
					}; 
				}
				catch
				{
					// ignored
				}
			}

			return CalculateHeight(property, label);
		}

		private float GetHeightMultiOpsString(SerializedProperty property, GUIContent label)
		{
			var	newValue = conditionField.GetValue(property.serializedObject.targetObject);

			if (lastValue != newValue)
			{
				lastValue = newValue;

				lastValueStringIfIed = lastValue != null 
				                       && (!typeof(Object).IsAssignableFrom(lastValue.GetType()) || (lastValue as Object)?.ToString() != "null")  // Unity Object is not referenced as real null, it is fake. Don't trust them. 
						? lastValue.ToString() 
						:  string.Empty;

				conditionResult = multiOperator switch
				{
					MultiOp.Equals => targetValueStringIfIed.Any(t => lastValueStringIfIed.Equals(t)) ? normalBooleanValue : !normalBooleanValue,
					MultiOp.Diff => targetValueStringIfIed.Any(t => lastValueStringIfIed.Equals(t)) ? !normalBooleanValue : normalBooleanValue,
					_ => conditionResult
				};
			}

			return CalculateHeight(property, label);
		}

		private float	GetHeightMultiOpsScalar(SerializedProperty property, GUIContent label)
		{
			var newValue = conditionField.GetValue(property.serializedObject.targetObject);

			if (newValue.Equals(lastValue) == false)
			{
				lastValue = newValue;

				try
				{
					var value = Convert.ToDecimal(newValue);

					conditionResult = multiOperator switch
					{
						MultiOp.Equals => targetValueDecimalIed.Any(t => value == t) ? normalBooleanValue : !normalBooleanValue,
						MultiOp.Diff => targetValueDecimalIed.Any(t => value == t) ? !normalBooleanValue : normalBooleanValue,
						_ => conditionResult
					};
				}
				catch
				{
					// ignored
				}
			}

			return CalculateHeight(property, label);
		}

		private float CalculateHeight(SerializedProperty property, GUIContent label)
		{
			if (conditionResult != normalBooleanValue) return EmptyHeight;
			if (!invalidHeight) return cachedHeight;
			
			invalidHeight = false;
			cachedHeight = EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
			return cachedHeight;
		}
	}
}
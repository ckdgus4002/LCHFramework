using System;
using System.Reflection;
using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ShowIfAttribute : PropertyAttribute, IIfAttribute
	{
		public ShowIfAttribute(string fieldName = "", ComparisonOperator comparisonOperator = ComparisonOperator.Equals, object comparisonValue = null)
		{
			FieldName = fieldName;
			ComparisonOperator = comparisonOperator;
			ComparisonValue = comparisonValue;
			Force = null;
		}
		
		public ShowIfAttribute(bool force)
		{
			FieldName = "";
			ComparisonOperator = ComparisonOperator.Equals;
			ComparisonValue = null;
			Force = force;
		}
		
		
		
		public string FieldName { get; }
		public ComparisonOperator ComparisonOperator { get; }
		public object ComparisonValue { get; }
		public bool? Force { get; }
		public FieldInfo FieldInfo { get; set;  }
	}
}
using System;
using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(StringEnumAttribute))]
    public class StringEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var strings = ((StringEnumAttribute)attribute).Strings;
            if (property.propertyType == SerializedPropertyType.String)
            {
                var index = EditorGUI.Popup(position, property.displayName, Math.Max(0, Array.IndexOf(strings, property.stringValue)), strings);
                property.stringValue = strings[index];
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                var index = EditorGUI.Popup(position, property.displayName, property.intValue, strings);;
                property.intValue = index;
            }
            else
                EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
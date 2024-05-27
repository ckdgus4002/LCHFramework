#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [CustomPropertyDrawer(typeof(StringInListAttribute))]
    public class StringInListDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var list = ((StringInListAttribute)attribute).List;
            if (property.propertyType == SerializedPropertyType.String)
            {
                var index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                index = EditorGUI.Popup(position, property.displayName, index, list);

                property.stringValue = list[index];
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
                property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
            else
                base.OnGUI(position, property, label);
        }
    }
}
#endif
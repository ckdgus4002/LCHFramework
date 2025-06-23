using UnityEditor;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [CustomPropertyDrawer(typeof(InspectorNameAttribute))]
    public class InspectorNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.text = ((InspectorNameAttribute)attribute).Name;
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }
    }
}
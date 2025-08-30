using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(EnableInInspectorAttribute))]
    public class EnableIfDrawer : IfDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var result = GetIfAttributeResult((IInInspectorAttribute)attribute, property);
            var prevEnabled = GUI.enabled;
            GUI.enabled = result;
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
            GUI.enabled = prevEnabled;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property, label);
    }   
}
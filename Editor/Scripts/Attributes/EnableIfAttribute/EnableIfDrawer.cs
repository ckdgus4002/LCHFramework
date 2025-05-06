using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfDrawer : IfDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var result = GetIfAttributeResult((IIfAttribute)attribute, property);
            var prevEnabled = GUI.enabled;
            GUI.enabled = result;
            if (result) EditorGUI.PropertyField(position, property, label, property.isExpanded);
            GUI.enabled = prevEnabled;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var result = GetIfAttributeResult((IIfAttribute)attribute, property);
            return !result ? EditorGUI.GetPropertyHeight(property, label) : -EditorGUIUtility.standardVerticalSpacing;
        }
    }   
}
using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(EnableInInspectorAttribute))]
    public class EnableInInspectorDrawer : InInspectorDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var result = GetInInspectorAttributeResult((IInInspectorAttribute)attribute, property);
            var prevEnabled = GUI.enabled;
            GUI.enabled = result;
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
            GUI.enabled = prevEnabled;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }   
}
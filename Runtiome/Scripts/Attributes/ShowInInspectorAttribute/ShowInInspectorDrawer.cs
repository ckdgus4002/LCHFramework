#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Attributes.ShowInInspector
{
    [CustomPropertyDrawer(typeof(ShowInInspectorAttribute))]
    public class ShowInInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var button = (ShowInInspectorAttribute)attribute;

            var methodInfo = button.GetMethod(property.serializedObject.targetObject);
            if (methodInfo != null && GUI.Button(rect, label))
                methodInfo.Invoke(property.serializedObject.targetObject, null);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => ((ShowInInspectorAttribute) attribute).GetMethod(property.serializedObject.targetObject) != null
                ? base.GetPropertyHeight(property, label)
                : -1
                ;
    }
}
#endif
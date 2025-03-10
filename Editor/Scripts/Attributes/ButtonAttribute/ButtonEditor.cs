using System.Linq;
using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Attributes
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawButtonsInspector(targets);
        }

        private void DrawButtonsInspector(Object[] objects)
        {
            var targetType = objects[0].GetType();
            foreach (var methodInfo in TypeCache.GetMethodsWithAttribute<ButtonAttribute>()
                         .Where(r => r.DeclaringType.IsAssignableFrom(targetType) || (targetType.BaseType.IsGenericType && targetType.BaseType.GetGenericTypeDefinition() == r.DeclaringType))
                         .OrderBy(r => r.MetadataToken))
                foreach (var attribute in methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true))
                {
                    var buttonAttribute = (ButtonAttribute)attribute;
                    buttonAttribute.labelName = string.IsNullOrEmpty(buttonAttribute.labelName) ? methodInfo.Name : buttonAttribute.labelName;
                    buttonAttribute.methodInfo = methodInfo;
                    DrawButtonInspector(buttonAttribute, objects);
                }
        }

        private void DrawButtonInspector(ButtonAttribute buttonAttribute, object[] objects)
        {
            if (!GUILayout.Button(buttonAttribute.labelName)) return;

            foreach (var t in objects) buttonAttribute.methodInfo.Invoke(t, null);
        }
    }
}
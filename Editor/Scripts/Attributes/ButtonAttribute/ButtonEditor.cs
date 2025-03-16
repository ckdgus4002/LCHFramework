using System.Linq;
using System.Reflection;
using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;
using EditorTypeUtility = LCHFramework.Editor.Utilities.TypeUtility;
using TypeUtility = LCHFramework.Utilities.TypeUtility;

namespace LCHFramework.Editor.Attributes
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawButtonsInspector(serializedObject.targetObjects);
        }

        private void DrawButtonsInspector(Object[] objects)
        {
            var targetType = objects[0].GetType();
            foreach (var methodInfo in EditorTypeUtility.GetMethodInfos(targetType))
            {
                var buttonAttribute = methodInfo.GetCustomAttribute<ButtonAttribute>(true);
                buttonAttribute.labelName = string.IsNullOrEmpty(buttonAttribute.labelName) ? methodInfo.Name : buttonAttribute.labelName;
                buttonAttribute.methodInfo = TypeUtility.GetMethodInfo(targetType, methodInfo.Name, methodInfo.GetParameters().Select(r => r.ParameterType).ToArray()) ?? methodInfo;
                if (GUILayout.Button(buttonAttribute.labelName))
                    foreach (var t in objects) buttonAttribute.methodInfo.Invoke(t, null);        
            }
        }
    }
}
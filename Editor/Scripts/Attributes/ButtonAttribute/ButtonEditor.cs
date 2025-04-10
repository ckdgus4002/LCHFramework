using System.Linq;
using System.Reflection;
using LCHFramework.Attributes;
using LCHFramework.Editor.Utilities;
using LCHFramework.Utilities;
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
            DrawButtonsInspector(serializedObject.targetObjects);
        }

        private void DrawButtonsInspector(Object[] objects)
        {
            if (objects[0] == null) return;
            
            var targetType = objects[0].GetType();
            // foreach (var methodInfo in targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(t => t.GetCustomAttributes<ButtonAttribute>(true).Any()))
            foreach (var methodInfo in TypeUtility<ButtonAttribute>.GetMethodInfos(targetType))
            {
                var buttonAttribute = methodInfo.GetCustomAttribute<ButtonAttribute>(true);
                buttonAttribute.labelName = string.IsNullOrEmpty(buttonAttribute.labelName) ? methodInfo.Name : buttonAttribute.labelName;
                // buttonAttribute.methodInfo = methodInfo;
                buttonAttribute.methodInfo = TypeUtility.GetMethodInfo(targetType, methodInfo.Name, methodInfo.GetParameters().Select(r => r.ParameterType).ToArray()) ?? methodInfo;
                if (!GUILayout.Button(buttonAttribute.labelName)) continue;
            
                foreach (var t in objects) buttonAttribute.methodInfo.Invoke(t, null);    
            }
        }
    }
}
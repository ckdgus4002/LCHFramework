using System.Reflection;
using LCHFramework.Attributes;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Attributes
{
    [CanEditMultipleObjects, CustomEditor(typeof(MonoBehaviour), true)]
    public class ShowInInspectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawButtonsInspector(targets);
        }

        private void DrawButtonsInspector(Object[] objects)
        {
            foreach (var methodInfo in objects[0].GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                foreach (var attribute in methodInfo.GetCustomAttributes(typeof(ShowInInspectorAttribute), true))
                {
                    var button = (ShowInInspectorAttribute)attribute;
                    button.labelName = string.IsNullOrWhiteSpace(button.labelName) ? methodInfo.Name : button.labelName;
                    button.methodInfo = methodInfo;
                    DrawButtonInspector(button, objects);
                }
            }
        }

        private void DrawButtonInspector(ShowInInspectorAttribute button, Object[] objects)
        {
            if (!GUILayout.Button(button.labelName)) return;
            
            foreach (var o in objects) button.methodInfo.Invoke(o, null);
        }
    }
}
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Attributes.ShowInInspector
{
    [CanEditMultipleObjects, CustomEditor(typeof(MonoBehaviour), true)]
    public class ShowInInspectorEditor : Editor
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
                    button.labelName = methodInfo.Name.Replace('_', ' ');

                    button.methodInfo = methodInfo;
                    DrawButtonInspector(button, objects);
                }
            }
        }

        private void DrawButtonInspector(ShowInInspectorAttribute button, Object[] objects)
        {
            if (!GUILayout.Button(button.labelName)) return;
            
            var methodInfo = string.IsNullOrWhiteSpace(button.methodName) 
                    ? button.methodInfo
                    : button.GetMethod(objects[0])
                    ;
            foreach (var o in objects) methodInfo.Invoke(o, null);
        }
    }
}
#endif
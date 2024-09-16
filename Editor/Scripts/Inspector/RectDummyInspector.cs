using LCHFramework.Components.UI;
using UnityEditor;
using UnityEngine;

namespace LCHFramework
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RectDummy))]
    public class RectDummyInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var raycast = serializedObject.FindProperty("m_RaycastTarget");

            GUILayout.Space(5);
            EditorGUILayout.PropertyField(raycast);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

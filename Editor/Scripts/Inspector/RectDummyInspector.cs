using LCHFramework.Components.UI;
using UnityEditor;

namespace LCHFramework.Editor.Inspector
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RectDummy))]
    public class RectDummyInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RaycastTarget"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}

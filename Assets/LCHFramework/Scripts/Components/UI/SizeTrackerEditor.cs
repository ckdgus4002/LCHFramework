#if UNITY_EDITOR
using UnityEditor;

namespace LCHFramework.Components.UI
{
    [CustomEditor(typeof(SizeTracker), true)]
    public class SizeTrackerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var sizeTracker = (SizeTracker)target;
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(sizeTracker.widthTarget)), true);
            if (sizeTracker.widthTarget != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(sizeTracker.leftPadding)), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(sizeTracker.rightPadding)), true);   
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(sizeTracker.heightTarget)), true);
            if (sizeTracker.heightTarget != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(sizeTracker.topPadding)), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(sizeTracker.bottomPadding)), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
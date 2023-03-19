using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RectDummy : Graphic, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(UnityEngine.Vector2 sp, Camera eventCamera)
        {
            if (!isActiveAndEnabled)
                return true;

            return RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, sp, eventCamera);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
    
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(RectDummy))]
    [UnityEditor.CanEditMultipleObjects]
    public class RectDummyInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var raycast = serializedObject.FindProperty("m_RaycastTarget");

            GUILayout.Space(5);
            UnityEditor.EditorGUILayout.PropertyField(raycast);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
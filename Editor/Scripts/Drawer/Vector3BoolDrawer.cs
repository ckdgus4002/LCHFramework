using LCHFramework.Data;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(Vector3Bool))]
    public class Vector3BoolDrawer : VectorBoolDrawer
    {
        protected override GUIContent[] Labels => _labels ??= new[]
        {
            new GUIContent("X"),
            new GUIContent("Y"),
            new GUIContent("Z")
        };
        private GUIContent[] _labels;
    }
}
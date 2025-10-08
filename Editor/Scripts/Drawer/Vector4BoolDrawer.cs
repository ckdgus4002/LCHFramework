using LCHFramework.Data;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(Vector4Bool))]
    public class Vector4BoolDrawer : VectorBoolDrawer
    {
        protected override GUIContent[] Labels => _labels ??= new[]
        {
            new GUIContent("X"),
            new GUIContent("Y"),
            new GUIContent("Z"),
            new GUIContent("W")
        };
        private GUIContent[] _labels;
    }
}
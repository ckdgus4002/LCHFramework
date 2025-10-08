using LCHFramework.Data;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Drawer
{
    [CustomPropertyDrawer(typeof(Vector2Bool))]
    public class Vector2BoolDrawer : VectorBoolDrawer
    {
        protected override GUIContent[] Labels => _labels ??= new[]
        {
            new GUIContent("X"),
            new GUIContent("Y"),
        };
        private GUIContent[] _labels;
    }   
}
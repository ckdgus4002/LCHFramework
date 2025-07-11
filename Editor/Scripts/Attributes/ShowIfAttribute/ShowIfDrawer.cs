﻿using UnityEditor;
using UnityEngine;

namespace LCHFramework.Attributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : IfDrawer
    {
        private bool invalidHeight = true;
        private float cachedHeight;
        
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var result = GetIfAttributeResult((IIfAttribute)attribute, property);
            if (!result) return;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
            invalidHeight = EditorGUI.EndChangeCheck() || invalidHeight;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var result = GetIfAttributeResult((IIfAttribute)attribute, property);
            if (!result) return -EditorGUIUtility.standardVerticalSpacing;

            if (!invalidHeight) return cachedHeight;
            
            invalidHeight = false;
            cachedHeight = EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
            return cachedHeight;
        }
    }
}
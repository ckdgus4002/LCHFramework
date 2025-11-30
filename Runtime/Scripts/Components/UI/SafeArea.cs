using System;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SafeArea : DrivenRectTransformBehaviour
    {
        public SizeMode sizeModeX = SizeMode.SafeArea;
        public SizeMode sizeModeY = SizeMode.SafeArea;
        
        
        [NonSerialized] private Vector2 _prevPosition;
        [NonSerialized] private Vector2 _prevSize;
        
        
        
        protected override void OnReset()
        {
            _prevPosition = new Vector2(float.MinValue, float.MinValue);
            _prevSize = new Vector2(float.MinValue, float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            if (RootCanvasOrNull == null || (RootCanvasOrNull.renderMode == RenderMode.WorldSpace && RootCanvasOrNull.worldCamera == null)) return false;
            
            var result = _prevPosition != UnityEngine.Screen.safeArea.position || _prevSize != UnityEngine.Screen.safeArea.size;
            _prevPosition = UnityEngine.Screen.safeArea.position;
            _prevSize = UnityEngine.Screen.safeArea.size;
            return result;
        }

        protected override void SetAll()
        {
            Tracker.Add(this, RectTransform, DrivenTransformProperties.All);

            RectTransform.anchorMin = Vector2Utility.Half;
            RectTransform.anchorMax = Vector2Utility.Half;
            RectTransform.pivot = Vector2Utility.Half;
            RectTransform.rotation = Quaternion.identity;
            RectTransform.localScale = Vector3.one;
            var scaleFactor = CanvasUtility.GetScaleFactor(RootCanvasOrNull);
            if (scaleFactor == 0) return;
            var insetAverage = new Vector2(UnityEngine.Screen.width - UnityEngine.Screen.safeArea.width, UnityEngine.Screen.height - UnityEngine.Screen.safeArea.height) * 0.5f;
            var position = new Vector2((sizeModeX == SizeMode.SafeArea ? (UnityEngine.Screen.safeArea.position.x - insetAverage.x) : 0 ) / scaleFactor, (sizeModeY == SizeMode.SafeArea ? (UnityEngine.Screen.safeArea.position.y - insetAverage.y) : 0 ) / scaleFactor);
            RectTransform.localPosition = new Vector2(float.IsInfinity(position.x) ? 0 : position.x, float.IsInfinity(position.y) ? 0 : position.y);
            var size = new Vector2((sizeModeX == SizeMode.SafeArea ? UnityEngine.Screen.safeArea.size.x : UnityEngine.Screen.width) / scaleFactor, (sizeModeY == SizeMode.SafeArea ? UnityEngine.Screen.safeArea.size.y : UnityEngine.Screen.height) / scaleFactor);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, float.IsInfinity(size.x) ? Width : size.x);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, float.IsInfinity(size.y) ? Height : size.y);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
        
        
        
        public enum SizeMode
        {
            SafeArea = 1,
            Screen,
        }
    }
}
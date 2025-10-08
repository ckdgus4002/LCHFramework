using System;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SafeArea : DrivenRectTransformBehaviour
    {
        [NonSerialized] private Vector2 _prevPosition;
        [NonSerialized] private Vector2 _prevSize;
        
        
        
        protected override void OnReset()
        {
            _prevPosition = new Vector2(float.MinValue, float.MinValue);
            _prevSize = new Vector2(float.MinValue, float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            if (RootCanvasOrNull == null || RootCanvasOrNull.worldCamera == null) return false;
            
            var result = _prevPosition != Screen.safeArea.position || _prevSize != Screen.safeArea.size;
            _prevPosition = Screen.safeArea.position;
            _prevSize = Screen.safeArea.size;
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
            var scaleFactor = RootCanvasOrNull.renderMode != RenderMode.WorldSpace ? RootCanvasOrNull.scaleFactor : Screen.height / (RootCanvasOrNull.worldCamera.orthographicSize * 2);
            if (scaleFactor == 0) return;
            var insetAverage = new Vector2(Screen.width - Screen.safeArea.width, Screen.height - Screen.safeArea.height) * 0.5f;
            var position = (Screen.safeArea.position - insetAverage) / scaleFactor;
            RectTransform.localPosition = new Vector2(float.IsInfinity(position.x) ? 0 : position.x, float.IsInfinity(position.y) ? 0 : position.y);
            var size = Screen.safeArea.size / scaleFactor;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, float.IsInfinity(size.x) ? RectTransform.rect.width : size.x);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, float.IsInfinity(size.y) ? RectTransform.rect.height : size.y);
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
        }
    }
}
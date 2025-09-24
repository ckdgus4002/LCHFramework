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
            var result = _prevPosition != Screen.safeArea.position || _prevSize != Screen.safeArea.size;
            _prevPosition = Screen.safeArea.position;
            _prevSize = Screen.safeArea.size;
            return result;
        }

        protected override void SetAll()
        {
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.All);

            RectTransformOrNull.anchorMin = Vector2Utility.Half;
            RectTransformOrNull.anchorMax = Vector2Utility.Half;
            RectTransformOrNull.pivot = Vector2Utility.Half;
            RectTransformOrNull.rotation = Quaternion.identity;
            RectTransformOrNull.localScale = Vector3.one;
            var scaleFactor = RootCanvasOrNull.renderMode != RenderMode.WorldSpace ? RootCanvasOrNull.scaleFactor : Screen.height / (RootCanvasOrNull.worldCamera.orthographicSize * 2);
            var insetAverage = new Vector2(Screen.width - Screen.safeArea.width, Screen.height - Screen.safeArea.height) * 0.5f;
            RectTransformOrNull.position = (Screen.safeArea.position - insetAverage) / scaleFactor;
            RectTransformOrNull.sizeDelta = Screen.safeArea.size / scaleFactor;
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}
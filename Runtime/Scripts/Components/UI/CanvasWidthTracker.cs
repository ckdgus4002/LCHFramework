using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasWidthTracker : LayoutSelfController
    {
        private float _prevAspect;


        private Canvas Canvas => _canvas == null ? _canvas = GetComponent<Canvas>() : _canvas;
        private Canvas _canvas;
        
        
        
        protected override bool HorizontalIsChanged()
        {
            if (Canvas.worldCamera == null) return false;
            
            var result = !Mathf.Approximately(_prevAspect, Canvas.worldCamera.aspect);
            _prevAspect = Canvas.worldCamera.aspect;
            return result;
        }

        public override void SetLayoutHorizontal()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);
            
            if (Canvas.worldCamera == null) return;
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Canvas.worldCamera.aspect * RectTransformOrNull.rect.height);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}

using LCHFramework.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SizeTracker : LayoutSelfController
    {
        public RectTransform widthTarget;
        [HideIf(nameof(widthTarget), null)] public float leftPadding;
        [HideIf(nameof(widthTarget), null)] public float rightPadding;
        public RectTransform heightTarget;
        [HideIf(nameof(heightTarget), null)] public float topPadding;
        [HideIf(nameof(heightTarget), null)] public float bottomPadding;
        
        
        private float _prevWidth = float.MinValue;
        private float _prevHeight = float.MinValue;
        
        
        
        protected override bool HorizontalIsChanged()
        {
            if (widthTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevWidth, widthTarget.rect.size.x);
            _prevWidth = widthTarget.rect.size.x;
            return result;
        }
        
        public override void SetLayoutHorizontal() => SetLayout();

        protected override bool VerticalIsChanged()
        {
            if (heightTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevHeight, heightTarget.rect.size.y);
            _prevHeight = heightTarget.rect.size.y;
            return result;
        }

        public override void SetLayoutVertical() => SetLayout();

        private void SetLayout()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, widthTarget != null && heightTarget != null ? DrivenTransformProperties.SizeDelta
                : widthTarget != null ? DrivenTransformProperties.SizeDeltaX
                : heightTarget != null ? DrivenTransformProperties.SizeDeltaY
                : DrivenTransformProperties.None
            );
            
            if (widthTarget != null) RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, leftPadding + widthTarget.rect.size.x + rightPadding);
            if (heightTarget != null) RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, topPadding + heightTarget.rect.size.y + bottomPadding);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}
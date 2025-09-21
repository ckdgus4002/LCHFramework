using LCHFramework.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SizeTrackerByTarget : LayoutSelfController
    {
        public RectTransform widthTarget;
        [ShowInInspector(nameof(widthTarget))] public float leftPadding;
        [ShowInInspector(nameof(widthTarget))] public float rightPadding;
        public RectTransform heightTarget;
        [ShowInInspector(nameof(heightTarget))] public float topPadding;
        [ShowInInspector(nameof(heightTarget))] public float bottomPadding;
        
        
        private float _prevWidth = float.MinValue;
        private float _prevHeight = float.MinValue;
        
        
        
        protected override bool SizeXIsChanged()
        {
            if (widthTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevWidth, widthTarget.rect.size.x);
            _prevWidth = widthTarget.rect.size.x;
            return result;
        }
        
        protected override void SetSizeX() => SetSize();

        protected override bool SizeYIsChanged()
        {
            if (heightTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevHeight, heightTarget.rect.size.y);
            _prevHeight = heightTarget.rect.size.y;
            return result;
        }

        protected override void SetSizeY() => SetSize();
        
        
        
        private void SetSize()
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
using System;
using LCHFramework.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SizeTrackerByTarget : DrivenRectTransformBehaviour
    {
        public RectTransform widthTarget;
        [ShowInInspector(nameof(widthTarget))] public float leftPadding;
        [ShowInInspector(nameof(widthTarget))] public float rightPadding;
        public RectTransform heightTarget;
        [ShowInInspector(nameof(heightTarget))] public float topPadding;
        [ShowInInspector(nameof(heightTarget))] public float bottomPadding;
        
        
        [NonSerialized] private float _prevWidth;
        [NonSerialized] private float _prevHeight;
        
        
        
        protected override void OnReset()
        {
            _prevWidth = float.MinValue;
            _prevHeight = float.MinValue;
        }
        
        
        
        protected override bool SizeXIsChanged()
        {
            if (widthTarget == null || heightTarget != null) return false;
            
            var result = !Mathf.Approximately(_prevWidth, widthTarget.rect.size.x);
            _prevWidth = widthTarget.rect.size.x;
            return result;
        }
        
        protected override void SetSizeX()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);
            
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, leftPadding + widthTarget.rect.size.x + rightPadding);
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }

        protected override bool SizeYIsChanged()
        {
            if (widthTarget != null || heightTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevHeight, heightTarget.rect.size.y);
            _prevHeight = heightTarget.rect.size.y;
            return result;
        }

        protected override void SetSizeY()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull,DrivenTransformProperties.SizeDeltaY);
            
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, topPadding + heightTarget.rect.size.y + bottomPadding);
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }

        protected override bool SizeIsChanged()
        {
            if (widthTarget == null || heightTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevWidth, widthTarget.rect.size.x) || !Mathf.Approximately(_prevHeight, heightTarget.rect.size.y);
            _prevWidth = widthTarget.rect.size.x;
            _prevHeight = heightTarget.rect.size.y;
            return result;
        }
        
        protected override void SetSize()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDelta);
            
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, leftPadding + widthTarget.rect.size.x + rightPadding);
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, topPadding + heightTarget.rect.size.y + bottomPadding);
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}
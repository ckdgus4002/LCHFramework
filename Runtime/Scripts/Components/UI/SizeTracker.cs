using System;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SizeTracker : DrivenRectTransformBehaviour
    {
        public RectTransform target;
        [ShowInInspector(nameof(target))] public Vector2Bool isReverseScale;
        
        
        [NonSerialized] private Vector2 _prevSize;
        [NonSerialized] private Vector3 _prevEulerAngles;
        [NonSerialized] private Vector3 _prevScale;
        
        
        
        protected override void OnReset()
        {
            _prevSize = Vector2Utility.New(float.MinValue);
            _prevEulerAngles = Vector3Utility.New(float.MinValue);
            _prevScale = Vector3Utility.New(float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            if (target == null) return false;
            
            var result = _prevSize != target.rect.size || _prevEulerAngles != target.eulerAngles || _prevScale != target.lossyScale;
            _prevSize = target.rect.size;
            _prevEulerAngles = target.eulerAngles;
            _prevScale = target.lossyScale;
            return result;
        }

        protected override void SetAll()
        {
            tracker.Clear();
            tracker.Add(this, RectTransform, DrivenTransformProperties.Anchors | DrivenTransformProperties.SizeDelta | DrivenTransformProperties.Rotation | DrivenTransformProperties.Scale);

            RectTransform.anchorMin = Vector2Utility.Half;
            RectTransform.anchorMax = Vector2Utility.Half;
            RectTransform.rotation = target.rotation;
            RectTransform.localScale = new Vector3(isReverseScale.x ? -1 : 1, isReverseScale.y ? -1 : 1, 1);
            var scaleFactor = RectTransform.parent.lossyScale;
            var size = target.rect.size / scaleFactor;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}
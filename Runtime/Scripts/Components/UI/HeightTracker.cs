using System;
using LCHFramework.Attributes;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class HeightTracker : DrivenRectTransformBehaviour
    {
        public RectTransform target;
        [ShowInInspector(nameof(target))] public bool isReverseScale;
        
        
        [NonSerialized] private float _prevHeight;
        [NonSerialized] private Vector3 _prevEulerAngles;
        [NonSerialized] private Vector3 _prevScale;
        
        
        
        protected override void OnReset()
        {
            _prevHeight = float.MinValue;
            _prevEulerAngles = Vector3Utility.New(float.MinValue);
            _prevScale = Vector3Utility.New(float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            if (target == null) return false;
            
            var result = !Mathf.Approximately(_prevHeight, target.rect.size.y) || _prevEulerAngles != target.eulerAngles || _prevScale != target.lossyScale;
            _prevHeight = target.rect.size.y;
            _prevEulerAngles = target.eulerAngles;
            _prevScale = target.lossyScale;
            return result;
        }
        
        protected override void SetAll()
        {
            tracker.Clear();
            tracker.Add(this, RectTransform, DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaY | DrivenTransformProperties.Rotation | DrivenTransformProperties.Scale);

            RectTransform.anchorMin = RectTransform.anchorMin.SetY(0.5f);
            RectTransform.anchorMax = RectTransform.anchorMin.SetY(0.5f);
            RectTransform.rotation = target.rotation;
            RectTransform.localScale = new Vector3(isReverseScale ? -1 : 1, 1, 1);
            var scaleFactor = RectTransform.parent.lossyScale.y;
            var size = target.rect.size.y / scaleFactor;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}
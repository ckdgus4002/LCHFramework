using System;
using LCHFramework.Attributes;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class WidthTracker : DrivenRectTransformBehaviour
    {
        public RectTransform target;
        [ShowInInspector(nameof(target))] public bool isReverseScale;
        
        
        [NonSerialized] private float _prevWidth;
        [NonSerialized] private Vector3 _prevEulerAngles;
        [NonSerialized] private Vector3 _prevScale;
        
        
        
        protected override void OnReset()
        {
            _prevWidth = float.MinValue;
            _prevEulerAngles = Vector3Utility.New(float.MinValue);
            _prevScale = Vector3Utility.New(float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            if (target == null) return false;
            
            var result = !Mathf.Approximately(_prevWidth, target.rect.size.x) || _prevEulerAngles != target.eulerAngles || _prevScale != target.lossyScale;
            _prevWidth = target.rect.size.x;
            _prevEulerAngles = target.eulerAngles;
            _prevScale = target.lossyScale;
            return result;
        }

        protected override void SetAll()
        {
            Tracker.Add(this, RectTransform, DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.Rotation | DrivenTransformProperties.Scale);

            RectTransform.anchorMin = RectTransform.anchorMin.SetX(0.5f);
            RectTransform.anchorMax = RectTransform.anchorMin.SetX(0.5f);
            RectTransform.rotation = target.rotation;
            RectTransform.localScale = new Vector3(1, isReverseScale ? -1 : 1, 1);
            var scaleFactor = RectTransform.parent.lossyScale.x;
            var size = target.rect.size.x / scaleFactor;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
        }
    }
}
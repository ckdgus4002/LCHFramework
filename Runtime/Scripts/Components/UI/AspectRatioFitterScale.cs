using System;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class AspectRatioFitterScale : DrivenRectTransformBehaviour
    {
        public AspectMode aspectMode = AspectMode.FitInParent;
        public float aspectRatio = 1;
        
        
        [NonSerialized] private Vector2 _prevParentSize;
        
        
        
        protected override void OnReset()
        {
            _prevParentSize = Vector2Utility.New(float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            switch (aspectMode)
            {
                case AspectMode.FitInParent:
                default:
                {
                    var parent = (RectTransform)RectTransform.parent;
                    if (parent.lossyScale == Vector3.zero) return false;
                    
                    var parentSize = parent.rect.size * parent.lossyScale;
                    var result = _prevParentSize != parentSize;
                    _prevParentSize = parentSize;
                    return result;
                }
            }
        }
        
        protected override void SetAll()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.All);
            
            switch (aspectMode)
            {
                case AspectMode.FitInParent:
                default:
                {
                    RectTransform.sizeDelta = Vector2.zero;
                    RectTransform.anchorMin = Vector2.zero;
                    RectTransform.anchorMax = Vector2.one;
                    RectTransform.anchoredPosition = Vector2.zero;
                    RectTransform.pivot = Vector2Utility.Half;
                    RectTransform.rotation = Quaternion.identity;
                    
                    var parent = (RectTransform)RectTransform.parent;
                    var parentSize = parent.rect.size * parent.lossyScale;
                    RectTransform.localScale = parentSize.y * aspectRatio < parentSize.x
                        ? new Vector3(GetSizeDeltaToProduceSize(parentSize.y * aspectRatio, parentSize, 0), 1, 1)
                        : new Vector3(1, GetSizeDeltaToProduceSize(parentSize.x / aspectRatio, parentSize, 1), 1);
                    break;
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
        
        private float GetSizeDeltaToProduceSize(float size, Vector2 parentSize, int axis)
        {
            return size / parentSize[axis];
        }
        
        
        
        public enum AspectMode
        {
            FitInParent = 3,
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class AspectRatioFitterScale : DrivenRectTransformBehaviour
    {
        public AspectMode aspectMode = AspectMode.FitInParent;
        public float aspectRatio = 1;
        
        
        
        protected override void OnReset()
        {
        }
        
        
        
        protected override bool AllIsChanged() => true;
        
        protected override void SetAll()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.All);
            
            switch (aspectMode)
            {
                case AspectMode.FitInParent:
                {
                    RectTransform.localPosition = Vector2.zero;
                    RectTransform.sizeDelta = Vector2.zero;
                    RectTransform.anchorMin = Vector2.zero;
                    RectTransform.anchorMax = Vector2.one;
                    
                    var scale = Vector3.one;
                    var parentSize = ((RectTransform)RectTransform.parent).rect.size;
                    if (parentSize.y * aspectRatio < parentSize.x)
                    {
                        scale.x = GetSizeDeltaToProduceSize(parentSize.y * aspectRatio, parentSize, 0);
                    }
                    else
                    {
                        scale.y = GetSizeDeltaToProduceSize(parentSize.x / aspectRatio, parentSize, 1);
                    }
                    RectTransform.localScale = scale;
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
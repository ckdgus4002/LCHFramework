using System;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class WidthControllerByScreenAspect : DrivenRectTransformBehaviour
    {
        [NonSerialized] private float _prevScreenAspect;
        
        
        
        protected override void OnReset()
        {
            _prevScreenAspect = float.MinValue;
        }
        
        
        
        protected override bool SizeXIsChanged()
        {
            var screenAspect = (float)Screen.width / Screen.height;
            var result = !Mathf.Approximately(_prevScreenAspect, screenAspect);
            _prevScreenAspect = screenAspect;
            return result;
        }
        
        protected override void SetSizeX()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.SizeDeltaX);

            var screenAspect = (float)Screen.width / Screen.height;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenAspect * Height);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}

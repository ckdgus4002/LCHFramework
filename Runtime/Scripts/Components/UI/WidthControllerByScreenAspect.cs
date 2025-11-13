using System;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class WidthControllerByScreenAspect : DrivenRectTransformBehaviour
    {
        [NonSerialized] private float _prevScreenAspectRatio;
        
        
        
        protected override void OnReset()
        {
            _prevScreenAspectRatio = float.MinValue;
        }
        
        
        
        protected override bool SizeXIsChanged()
        {
            var screenAspectRatio = Screen.AspectRatio;
            var result = !Mathf.Approximately(_prevScreenAspectRatio, screenAspectRatio);
            _prevScreenAspectRatio = screenAspectRatio;
            return result;
        }
        
        protected override void SetSizeX()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.SizeDeltaX);
            
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.AspectRatio * Height);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}

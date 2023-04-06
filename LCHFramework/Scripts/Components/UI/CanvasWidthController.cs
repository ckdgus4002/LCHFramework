using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public class CanvasWidthController : LayoutSelfController
    {
        private float _prevAspect = -1;
        
        
        
        protected override bool HorizontalIsChanged()
        {
            if (Camera.main == null) return false;
            
            var result = !Mathf.Approximately(_prevAspect, Camera.main.aspect);
            _prevAspect = Camera.main.aspect;
            return result;
        }

        public override void SetLayoutHorizontal()
        {
            tracker.Clear();
            tracker.Add(this, RectTransform, DrivenTransformProperties.SizeDeltaX);
            
            if (Camera.main == null) return;
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Camera.main.aspect * 1080);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
        }
    }
}
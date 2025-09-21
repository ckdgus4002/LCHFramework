using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class WidthControllerByScreenAspect : LayoutSelfController
    {
        private float screenAspect;
        private float _prevScreenAspect;
        
        
        
        protected override bool SizeXIsChanged()
        {
            var result = !Mathf.Approximately(_prevScreenAspect, screenAspect = (float)Screen.width / Screen.height);
            _prevScreenAspect = screenAspect;
            return result;
        }
        
        protected override void SetSizeX()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);

            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenAspect * RectTransformOrNull.rect.height);
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}

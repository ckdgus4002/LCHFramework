using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public class CanvasWidthController : LayoutSelfController
    {
        private float _prevAspect = -1;
        
        
        private LCHMonoBehaviour LCHMonoBehaviour => LCHMonoBehaviour.GetOrAddComponent(gameObject);
        
        
        
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
            tracker.Add(this, LCHMonoBehaviour.RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);
            
            if (Camera.main == null) return;
            LCHMonoBehaviour.RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Camera.main.aspect * LCHFrameworkSettings.Instance.canvasSize.y);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(LCHMonoBehaviour.RectTransformOrNull);
        }
    }
}
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public class RectTransformWidthController : LayoutSelfController
    {
        private Camera CameraOrNull
        {
            get
            {
                if (_camera == null && RootCanvasOrNull != null) _camera = RootCanvasOrNull.worldCamera;
                
                return _camera;
            }
        }
        private Camera _camera;
        
        
        
        private float _prevAspect = -1;
        protected override bool HorizontalIsChanged()
        {
            if (CameraOrNull == null) return false;

            var result = CameraUtility.IsAspectChanged(CameraOrNull, _prevAspect);
            _prevAspect = CameraOrNull.aspect;
            return result;
        }

        public override void SetLayoutHorizontal()
        {
            tracker.Clear();
            tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);
            
            if (CameraOrNull == null) return;
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CameraOrNull.aspect * LCHFramework.Instance.targetScreenResolution.y);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}
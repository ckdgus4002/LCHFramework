using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public class CanvasWidthController : LayoutSelfController
    {
        private Camera CameraOrNull
        {
            get
            {
                if (_camera == null && LCHMonoBehaviour.RootCanvasOrNull != null) _camera = LCHMonoBehaviour.RootCanvasOrNull.worldCamera;
                
                return _camera;
            }
        }
        private Camera _camera;
        
        
        private LCHMonoBehaviour LCHMonoBehaviour => _lchMonoBehaviour == null ? _lchMonoBehaviour = gameObject.GetOrAddComponent<LCHMonoBehaviour>() : _lchMonoBehaviour;
        private LCHMonoBehaviour _lchMonoBehaviour;
        
        
        
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
            tracker.Add(this, LCHMonoBehaviour.RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);
            
            if (CameraOrNull == null) return;
            LCHMonoBehaviour.RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CameraOrNull.aspect * LCHFramework.Instance.targetScreenResolution.y);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(LCHMonoBehaviour.RectTransformOrNull);
        }
    }
}
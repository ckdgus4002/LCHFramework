using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class WidthController : LayoutSelfController
    {
        [SerializeField] private ControlMode controlMode;
        
        
        private float _prevAspect;
        
        
        
        protected override bool HorizontalIsChanged()
        {
            if (controlMode == ControlMode.MainCameraAspect)
            {
                if (Camera.main != null) return false;
                
                var result = !Mathf.Approximately(_prevAspect, Camera.main.aspect);
                _prevAspect = Camera.main.aspect;
                return result;
            }
            else
                return false;
        }

        public override void SetLayoutHorizontal()
        {
            Tracker.Clear();
            if (controlMode != ControlMode.None) Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);

            if (controlMode == ControlMode.MainCameraAspect)
            {
                if (Camera.main == null) return;
                RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Camera.main.aspect * RectTransformOrNull.rect.height);    
            }
            
            if (controlMode != ControlMode.None && GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
        
        
        
        private enum ControlMode
        {
            None,
            MainCameraAspect,
        }
    }
}

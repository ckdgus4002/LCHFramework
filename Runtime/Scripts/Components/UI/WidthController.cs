using LCHFramework.Attributes;
using LCHFramework.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class WidthController : LayoutSelfController
    {
        [SerializeField] private ControlMode controlMode;
        
        
        private float _prevAspect;
        
        
        private Camera Camera => _camera == null ? _camera = Camera.main : _camera;
        [ShowInInspector(nameof(controlMode), ComparisonOperator.Equals, ControlMode.CameraAspect)] [SerializeField] private Camera _camera; 
        
        
        
        protected override bool HorizontalIsChanged()
        {
            if (controlMode == ControlMode.CameraAspect)
            {
                if (Camera == null) return false;
                
                var result = !Mathf.Approximately(_prevAspect, Camera.aspect);
                _prevAspect = Camera.aspect;
                return result;
            }
            else
                return false;
        }

        public override void SetLayoutHorizontal()
        {
            Tracker.Clear();
            if (controlMode != ControlMode.None) Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDeltaX);

            if (controlMode == ControlMode.CameraAspect)
            {
                if (Camera == null) return;
                RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Camera.aspect * RectTransformOrNull.rect.height);    
            }
            
            if (controlMode != ControlMode.None && GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
        
        
        
        private enum ControlMode
        {
            None,
            CameraAspect,
        }
    }
}

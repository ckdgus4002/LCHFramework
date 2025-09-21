using LCHFramework.Attributes;
using LCHFramework.Data;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class AspectRatioFitterController : MonoBehaviour
    {
        [SerializeField] private ControlMode controlMode;
        
        
        private Camera Camera => _camera == null ? _camera = Camera.main : _camera;
        [ShowInInspector(nameof(controlMode), ComparisonOperator.Equals, ControlMode.CameraAspect)] [SerializeField] private Camera _camera;
        
        private AspectRatioFitter AspectRatioFitter => _aspectRatioFitter == null ? GetComponent<AspectRatioFitter>() : _aspectRatioFitter;
        private AspectRatioFitter _aspectRatioFitter;
        
        
        
        private void Update() => AspectRatioFitter.aspectRatio = controlMode == ControlMode.CameraAspect ? Camera.aspect 
            : 1;
        
        
        
        enum ControlMode
        {
            None,
            CameraAspect
        }
    }
}
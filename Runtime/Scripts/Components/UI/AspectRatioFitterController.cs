using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class AspectRatioFitterController : MonoBehaviour
    {
        [SerializeField] private ControlMode controlMode;
        
        
        private AspectRatioFitter AspectRatioFitter => _aspectRatioFitter == null ? GetComponent<AspectRatioFitter>() : _aspectRatioFitter;
        private AspectRatioFitter _aspectRatioFitter;
        
        
        
        private void Update() => AspectRatioFitter.aspectRatio = controlMode == ControlMode.MainCameraAspect ? Camera.main.aspect
            : 1;
        
        
        
        enum ControlMode
        {
            None,
            MainCameraAspect
        }
    }
}
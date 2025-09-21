using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasScalerController : MonoBehaviour
    {
        private CanvasScaler CanvasScaler => _canvasScaler == null ? _canvasScaler = GetComponent<CanvasScaler>() : _canvasScaler; 
        private CanvasScaler _canvasScaler;
        
        
        
        private void Update()
        {
            CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            var targetScreenResolution = LCHFramework.Instance.targetScreenResolution;
            CanvasScaler.referenceResolution = targetScreenResolution;
            CanvasScaler.matchWidthOrHeight = targetScreenResolution.x / targetScreenResolution.y < 1 ? 0 : Screen.width == Screen.height ? .5f : 1;   
        }
    }
}
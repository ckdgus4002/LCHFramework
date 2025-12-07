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
            CanvasScaler.referenceResolution = LCHFramework.Instance.targetScreenResolution;
            CanvasScaler.matchWidthOrHeight = Screen.AspectRatio < 1 || (Mathf.Approximately(Screen.AspectRatio, 1) && !LCHFramework.Instance.isPreferredLandOrientation) ? 0 : 1;
        }
    }
}
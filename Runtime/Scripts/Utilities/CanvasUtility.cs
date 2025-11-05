using LCHFramework.Data;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class CanvasUtility
    {
        public static float GetScaleFactor(Canvas canvas) => canvas.scaleFactor;
        
        public static float GetScaleFactor() => GetScaleFactor(OrientationManager.Instance.Orientation.Value);
        
        public static float GetScaleFactor(Orientation orientation)  => orientation switch
        {
            >= Orientation.Portrait and <= Orientation.PortraitUpsideDown => Screen.width / LCHFramework.Instance.targetScreenResolution.x,
            >= Orientation.LandscapeLeft and <= Orientation.LandscapeRight => Screen.height / LCHFramework.Instance.targetScreenResolution.y,
            _ => -1
        };
    }
}
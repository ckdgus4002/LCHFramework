using LCHFramework.Data;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class CanvasUtility
    {
        public static float GetScaleFactor(Canvas canvas) => canvas.scaleFactor;
        
        public static float GetScaleFactor() => GetScaleFactor(OrientationManager.Instance.Orientation.Value, LCHFramework.Instance.targetScreenResolution);
        
        public static float GetScaleFactor(Vector2 targetScreenResolution) => GetScaleFactor(targetScreenResolution.x <= targetScreenResolution.y ? Orientation.Portrait : Orientation.LandscapeLeft, targetScreenResolution);
        
        public static float GetScaleFactor(Orientation orientation, Vector2 targetScreenResolution) => orientation switch
        {
            >= Orientation.Portrait and <= Orientation.PortraitUpsideDown => Screen.width / targetScreenResolution.x,
            >= Orientation.LandscapeLeft and <= Orientation.LandscapeRight => Screen.height / targetScreenResolution.y,
            _ => -1
        };
    }
}
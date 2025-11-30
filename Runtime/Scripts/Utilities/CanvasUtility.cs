using LCHFramework.Data;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class CanvasUtility
    {
        public static float GetScaleFactor(Canvas canvas) => canvas.renderMode != RenderMode.WorldSpace ? canvas.scaleFactor : GetScaleFactor(LCHFramework.Instance.targetScreenResolution);
        
        public static float GetScaleFactor(Vector2 screenSize)
        {
            var orientation 
                = OrientationManager.InstanceIsNull && screenSize.x <= screenSize.y ? Orientation.Portrait
                : OrientationManager.InstanceIsNull && screenSize.y < screenSize.x ? Orientation.LandscapeLeft
                : OrientationManager.Instance.Orientation.Value;
            return orientation switch
            {
                >= Orientation.Portrait and <= Orientation.PortraitUpsideDown => Screen.width / screenSize.x,
                >= Orientation.LandscapeLeft and <= Orientation.LandscapeRight => Screen.height / screenSize.y,
                _ => -1
            };
        }
    }
}
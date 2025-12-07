using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class CanvasUtility
    {
        public static float GetScaleFactor(Canvas canvas) => canvas.renderMode != RenderMode.WorldSpace ? canvas.scaleFactor : GetScaleFactor(LCHFramework.Instance.targetScreenResolution);
        
        public static float GetScaleFactor(Vector2 screenSize)
        {
            var screenAspectRatio = screenSize.x / screenSize.y;
            return screenAspectRatio < 1 || (Mathf.Approximately(screenAspectRatio, 1) && !LCHFramework.Instance.isPreferredLandOrientation) ? Screen.width / screenSize.x : Screen.height / screenSize.y;
        }
    }
}
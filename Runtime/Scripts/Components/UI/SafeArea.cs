using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    public class SafeArea : LCHMonoBehaviour, IScreenSizeChanged
    {
        // LCHFramework Event.
        public void OnScreenSizeChanged(Vector2 prev, Vector2 current)
        {
            Canvas.ForceUpdateCanvases();
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.safeArea.width * RootCanvasOrNull.scaleFactor);
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.safeArea.height * RootCanvasOrNull.scaleFactor);
        }
    }
}
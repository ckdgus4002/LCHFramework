using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    public class SafeArea : LCHMonoBehaviour, IScreenSizeChanged
    {
        // LCHFramework Event.
        public void OnScreenSizeChanged(Vector2 prev, Vector2 current)
        {
            Canvas.ForceUpdateCanvases();
            var scaleFactor = RootCanvasOrNull.scaleFactor.Reverse();
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.safeArea.width * scaleFactor);
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.safeArea.height * scaleFactor);
        }
    }
}
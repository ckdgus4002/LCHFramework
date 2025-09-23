using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : DrivenRectTransformBehaviour
    {
        protected virtual void LateUpdate()
        {
            Tracker.Clear();
            
            SetSize();
            SetAnchoredPosition();
        }
        
        
        
        private void SetSize()
        {
            if (RootCanvasOrNull == null) return;
            if (RootCanvasOrNull.renderMode == RenderMode.WorldSpace && LCHFramework.InstanceIsNull) return;
            
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDelta);
            var rootCanvasSize = ((RectTransform)RootCanvasOrNull.transform).rect.size;
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.safeArea.width * (RootCanvasOrNull.renderMode == RenderMode.WorldSpace
                ? rootCanvasSize.x / Screen.width
                : RootCanvasOrNull.scaleFactor.Reverse()));
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.safeArea.height * (RootCanvasOrNull.renderMode == RenderMode.WorldSpace
                ? rootCanvasSize.y / Screen.height
                : RootCanvasOrNull.scaleFactor.Reverse()));
        }
        
        protected virtual Vector2 GetAnchoredPosition()
            => OrientationManager.InstanceIsNull || OrientationManager.Instance.Orientation.Value is < Orientation.Portrait or > Orientation.LandscapeRight
                ? Vector2.zero
                : Screen.safeArea.center - ScreenUtility.HalfSize;
        
        private void SetAnchoredPosition()
        {
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.AnchoredPosition);
            RectTransformOrNull.anchoredPosition = GetAnchoredPosition();
        }
    }
}
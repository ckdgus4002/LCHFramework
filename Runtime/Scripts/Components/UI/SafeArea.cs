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
            
            var scaleFactor = RootCanvasOrNull.scaleFactor.Reverse();
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.SizeDelta);
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.safeArea.width * scaleFactor);
            RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.safeArea.height * scaleFactor);
        }
        
        private void SetAnchoredPosition()
        {
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.AnchoredPosition);
            RectTransformOrNull.anchoredPosition = GetAnchoredPosition();
        }
        
        protected virtual Vector2 GetAnchoredPosition()
        {
            var orientationIndex = OrientationManager.Instance.GetScreenOrientationIndex();
            return orientationIndex is < 1 or > 4 ? Vector2.zero : Screen.safeArea.center - ScreenUtility.HalfSize; 
        }
    }
}
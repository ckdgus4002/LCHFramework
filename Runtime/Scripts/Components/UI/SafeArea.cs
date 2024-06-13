using System;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    public class SafeArea : DrivenRectTransformBehaviour
    {
        protected int CurrentDeviceOrientationIndex { get; private set; } = -1;
        protected int PrevDeviceOrientationIndex { get; private set; } = -1;
        
        
        
        protected override void Start()
        {
            base.Start();

            CurrentDeviceOrientationIndex = (int)Input.deviceOrientation;
        }
        
        protected virtual void Update()
        {
            if (CurrentDeviceOrientationIndex == (int)Input.deviceOrientation) return;
            
            SetOrientationIndexes((int)Input.deviceOrientation);
        }
        
        private void OnRectTransformDimensionsChange()
        {
            Tracker.Clear();
            
            SetSize();
            SetAnchoredPosition();
        }



        protected void SetOrientationIndexes(int currentDeviceOrientationIndex)
        {
            PrevDeviceOrientationIndex = CurrentDeviceOrientationIndex;
            CurrentDeviceOrientationIndex = currentDeviceOrientationIndex;
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
            var orientationIndex = GetOrientationIndex();
            if (orientationIndex is < 1 or > 4) return Vector2.zero;

            var scaleFactor = Mathf.Min(Screen.width / Screen.safeArea.width, Screen.height / Screen.safeArea.height);
            var safeArea = Screen.safeArea.size * scaleFactor;
            return orientationIndex switch
            {
                1 => new Vector2(Screen.width - safeArea.x, safeArea.y - Screen.height),
                2 => new Vector2(Screen.width - safeArea.x, Screen.height - safeArea.y),
                3 => new Vector2(Screen.width - safeArea.x, Screen.height - safeArea.y),
                4 => new Vector2(safeArea.x - Screen.width, Screen.height - safeArea.y),
                _ => throw new ArgumentOutOfRangeException(nameof(orientationIndex), orientationIndex, null)
            };
        }
        
        protected virtual int GetOrientationIndex() => Screen.orientation < ScreenOrientation.AutoRotation ? (int)Screen.orientation 
            : DeviceOrientation.Portrait <= Input.deviceOrientation && Input.deviceOrientation <= DeviceOrientation.LandscapeRight ? (int)Input.deviceOrientation 
            : DeviceOrientation.FaceUp <= Input.deviceOrientation && Input.deviceOrientation <= DeviceOrientation.FaceDown ? PrevDeviceOrientationIndex 
            : -1;
    }
}
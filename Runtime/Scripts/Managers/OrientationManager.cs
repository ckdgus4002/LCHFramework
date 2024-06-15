using System;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class OrientationManager : MonoSingleton<OrientationManager>
    {
        public event Action<int, int> OnSetDeviceOrientationIndexes; // Prev, Current
        
        
        public int CurrentDeviceOrientationIndex { get; private set; } = -1;
        public int PrevDeviceOrientationIndex { get; private set; } = -1;
        
        
        
        protected override void Start()
        {
            base.Start();
            
            CurrentDeviceOrientationIndex = (int)Input.deviceOrientation;
        }
        
        protected virtual void Update()
        {
            if (CurrentDeviceOrientationIndex == (int)Input.deviceOrientation) return;
            
            SetDeviceOrientationIndexes((int)Input.deviceOrientation);
        }
        
        
        
        public void SetDeviceOrientationIndexes(int currentDeviceOrientationIndex)
        {
            PrevDeviceOrientationIndex = CurrentDeviceOrientationIndex;
            CurrentDeviceOrientationIndex = currentDeviceOrientationIndex;
            
            OnSetDeviceOrientationIndexes?.Invoke(PrevDeviceOrientationIndex, CurrentDeviceOrientationIndex);
        }
        
        public virtual int GetScreenOrientationIndex() => Screen.orientation < ScreenOrientation.AutoRotation ? (int)Screen.orientation 
            : DeviceOrientation.Portrait <= Input.deviceOrientation && Input.deviceOrientation <= DeviceOrientation.LandscapeRight ? (int)Input.deviceOrientation 
            : DeviceOrientation.FaceUp <= Input.deviceOrientation && Input.deviceOrientation <= DeviceOrientation.FaceDown ? PrevDeviceOrientationIndex 
            : -1;
    }
}

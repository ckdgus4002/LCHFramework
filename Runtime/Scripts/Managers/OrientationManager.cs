using System;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class OrientationManager : MonoSingleton<OrientationManager>
    {
        public event Action<ScreenOrientation, ScreenOrientation> OnOrientationChanged; // Prev, Current
        
        
        public ScreenOrientation PrevOrientation { get; private set; }
        
        
        public virtual ScreenOrientation Orientation
        {
            get => _orientation;
            private set
            {
                PrevOrientation = _orientation;
                _orientation = value;
                OnOrientationChanged?.Invoke(PrevOrientation, Orientation);
            }
        }
        private ScreenOrientation _orientation;
        
        

        protected override void Awake()
        {
            base.Awake();
            
            OnOrientationChanged += (prev, current) => Debug.Log($"Orientation Changed() PrevOrientation: {prev}, CurrentOrientation: {current}");
        }
    
        private void Update()
        {
            if (Screen.orientation == ScreenOrientation.AutoRotation && Input.deviceOrientation < DeviceOrientation.Portrait && DeviceOrientation.LandscapeRight < Input.deviceOrientation) return;
            
            var nextOrientation = Screen.orientation != ScreenOrientation.AutoRotation ? Screen.orientation : (ScreenOrientation)Input.deviceOrientation;
            if (Orientation == nextOrientation) return;

            Orientation = nextOrientation;
        }
    }
}

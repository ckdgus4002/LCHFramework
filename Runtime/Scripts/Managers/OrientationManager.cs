using LCHFramework.Data;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class OrientationManager : MonoSingleton<OrientationManager>
    {
        protected override bool IsDontDestroyOnLoad => true;
        
        public ReactiveProperty<Orientation> Orientation { get; } = new();
        
        
        
        private void Update()
        {
            var orientationIndex = Screen.orientation != ScreenOrientation.AutoRotation ? (int)Screen.orientation : (int)Input.deviceOrientation;
            if (orientationIndex is < 1 or > 4) return;
            
            Orientation.Value = (Orientation)orientationIndex;
        }
    }
}

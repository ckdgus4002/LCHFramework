using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class ScreenOrientationManager : ScreenOrientationManager<ScreenOrientationManager>
    {
    }
    
    [ExecuteAlways]
    public class ScreenOrientationManager<T> : MonoSingleton<T> where T : ScreenOrientationManager<T>
    {
        public ReactiveProperty<ScreenOrientation> Value { get; } = new();
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public override bool IsDestroyPrevInstance => false;
        
        protected virtual bool? IsPreferredLandscapeOrientation => LCHFramework.InstanceIsNull ? null : LCHFramework.Instance.isPreferredLandscapeOrientation;
        
        
        
        protected virtual void Update()
        {
            var screenAspectRatio = Screen.AspectRatio;
            switch (UnityEngine.Application.isEditor)
            {
                case true:
                {
                    var isScreenAspectRatioOne = Mathf.Approximately(screenAspectRatio, 1);
                    var isPreferredLandscapeOrientation = IsPreferredLandscapeOrientation;
                    SetScreenOrientation(screenAspectRatio < 1 || (isScreenAspectRatioOne && isPreferredLandscapeOrientation != null && !(bool)isPreferredLandscapeOrientation) ? ScreenOrientation.Portrait
                        : isScreenAspectRatioOne && isPreferredLandscapeOrientation == null ? ScreenOrientation.Unknown
                        : ScreenOrientation.LandscapeLeft);
                    break;
                }
                case false:
                {
                    var orientationIndex = UnityEngine.Screen.orientation != UnityEngine.ScreenOrientation.AutoRotation ? (int)UnityEngine.Screen.orientation : (int)Input.deviceOrientation;
                    SetScreenOrientation((int)ScreenOrientation.LandscapeRight < orientationIndex ? Value.Value : (ScreenOrientation)orientationIndex);
                    break;
                }
            }
        }
        
        
        
        protected virtual void SetScreenOrientation(ScreenOrientation value) => Value.Value = value;
    }
}
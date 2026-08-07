using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class OrientationManager : OrientationManager<OrientationManager>
    {
    }
    
    [ExecuteAlways]
    public class OrientationManager<T> : MonoSingleton<T> where T : OrientationManager<T>
    {
        public ReactiveProperty<ScreenOrientation> Orientation { get; } = new();
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public override bool IsDestroyPrevInstance => false;
        
        protected virtual bool? IsPreferredLandscapeOrientation => LCHFramework.InstanceIsNull ? null : LCHFramework.Instance.isPreferredLandscapeOrientation;
        
        protected int OrientationIndex => (int)Orientation.Value;
        
        
        
        protected virtual void Update()
        {
            var screenAspectRatio = Screen.AspectRatio;
            switch (UnityEngine.Application.isEditor)
            {
                case true:
                {
                    var isScreenAspectRatioOne = Mathf.Approximately(screenAspectRatio, 1);
                    var isPreferredLandscapeOrientation = IsPreferredLandscapeOrientation;
                    Orientation.Value = screenAspectRatio < 1 || (isScreenAspectRatioOne && isPreferredLandscapeOrientation != null && !(bool)isPreferredLandscapeOrientation) ? ScreenOrientation.Portrait
                        : isScreenAspectRatioOne && isPreferredLandscapeOrientation == null ? ScreenOrientation.Unknown
                        : ScreenOrientation.LandscapeLeft;
                    break;
                }
                case false:
                {
                    var unityEnginOrientationIndex = UnityEngine.Screen.orientation != UnityEngine.ScreenOrientation.AutoRotation ? (int)UnityEngine.Screen.orientation : (int)Input.deviceOrientation;
                    Orientation.Value = unityEnginOrientationIndex is < 1 or > 4 ? Orientation.Value : (ScreenOrientation)unityEnginOrientationIndex;
                    break;
                }
            }
        }
    }
}
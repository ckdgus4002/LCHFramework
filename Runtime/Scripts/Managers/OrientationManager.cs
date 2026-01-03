using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    [ExecuteAlways]
    public class OrientationManager : MonoSingleton<OrientationManager>
    {
        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitializeOnLoadMethod() => CreateGameObjectIfInstanceIsNull();
        
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public override bool IsDestroyPrevInstance => false;
        
        public ReactiveProperty<ScreenOrientation> Orientation { get; } = new();
        
        
        
        private void Update()
        {
#if UNITY_EDITOR
            Orientation.Value = Screen.AspectRatio < 1 || (Mathf.Approximately(Screen.AspectRatio, 1) && !LCHFramework.Instance.isPreferredLandscapeOrientation) ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft;
#else
            var orientationIndex = UnityEngine.Screen.orientation != UnityEngine.ScreenOrientation.AutoRotation ? (int)UnityEngine.Screen.orientation : (int)Input.deviceOrientation;
            Orientation.Value = orientationIndex is < 1 or > 4 ? Orientation.Value : (ScreenOrientation)orientationIndex;
#endif
        }
    }
}

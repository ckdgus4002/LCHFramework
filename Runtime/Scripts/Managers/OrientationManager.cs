using LCHFramework.Data;
using UniRx;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Managers
{
    [ExecuteAlways]
    public class OrientationManager : MonoSingleton<OrientationManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod() => CreateGameObjectIfInstanceIsNull();
        
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public override bool IsDestroyPrevInstance => false;
        
        public ReactiveProperty<Orientation> Orientation { get; } = new();
        
        
        
        private void Update()
        {
#if UNITY_EDITOR
            if (UnityEngine.Application.isEditor)
            {
                var mainGameViewSize = Handles.GetMainGameViewSize();
                Orientation.Value = mainGameViewSize.x <= mainGameViewSize.y ? Data.Orientation.Portrait : Data.Orientation.LandscapeLeft;
                return;
            }
#endif
            var orientationIndex = Screen.orientation != ScreenOrientation.AutoRotation ? (int)Screen.orientation : (int)Input.deviceOrientation;
            if (orientationIndex is > 0 and < 5)
            {
                Orientation.Value = (Orientation)orientationIndex;
                return;
            }
        }
    }
}

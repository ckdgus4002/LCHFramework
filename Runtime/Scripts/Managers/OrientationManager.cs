using LCHFramework.Data;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class OrientationManager : MonoSingleton<OrientationManager>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod() => CreateGameObjectIfInstanceIsNull();
        
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public ReactiveProperty<Orientation> Orientation { get; } = new();
        
        
        
        private void Update()
        {
            var orientationIndex = Screen.orientation != ScreenOrientation.AutoRotation ? (int)Screen.orientation : (int)Input.deviceOrientation;
            if (orientationIndex is < 1 or > 4) return;
            
            Orientation.Value = (Orientation)orientationIndex;
        }
    }
}

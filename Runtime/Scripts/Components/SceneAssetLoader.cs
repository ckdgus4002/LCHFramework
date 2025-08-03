using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Components
{
    public class SceneAssetLoader : LCHMonoBehaviour
    {
#if UNITY_EDITOR
        public SceneAsset scene;
#endif
        
        
        [HideInInspector] [SerializeField] private string sceneName;
        private AsyncOperation loadAsyncOrNull;
        
        
        private bool IsLoaded => loadAsyncOrNull is { isDone: true, progress: > 1 - float.Epsilon };
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            sceneName = scene != null ? scene.name : string.Empty;
        }
#endif
        
        
        
        // UnityEvent event.
        public void OnClick() => LoadAsync();
        
        
        
        public virtual AsyncOperation LoadAsync()
        {
            if (!IsLoaded) loadAsyncOrNull = SceneManager.LoadSceneAsync(sceneName);

            return loadAsyncOrNull;
        }
    }
}
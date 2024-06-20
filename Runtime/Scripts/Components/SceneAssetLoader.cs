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
        
        [SerializeField] private string sceneName;
        
        
        private bool IsLoaded => _loadAsync is { isDone: true, progress: > 1 - float.Epsilon };
        
        
        
        // UnityEvent event.
        public void OnClick() => LoadAsync();
        
        
        
        private void OnValidate()
        {
#if UNITY_EDITOR
            sceneName = scene != null ? scene.name : string.Empty;
#endif
        }
        
        
        private AsyncOperation _loadAsync;
        public AsyncOperation LoadAsync()
        {
            if (!IsLoaded) _loadAsync = SceneManager.LoadSceneAsync(sceneName);

            return _loadAsync;
        }
    }
}
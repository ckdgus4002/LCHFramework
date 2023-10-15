using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Components
{
    public class SceneAssetLoader : EditorObjectAllocator
    {
#if UNITY_EDITOR
        public SceneAsset scene;
#endif
        
        private string sceneName;
        
        
        private bool IsLoaded => _loadAsync != null && _loadAsync.isDone && 1 - float.Epsilon < _loadAsync.progress;
        
        
        
        // UnityEvent event.
        public void OnClick() => LoadAsync();
        
        
        
        public override void OnAllocate() => sceneName = scene != null ? scene.name : string.Empty;
        
        private AsyncOperation _loadAsync;
        public AsyncOperation LoadAsync()
        {
            if (!IsLoaded) _loadAsync = SceneManager.LoadSceneAsync(sceneName);

            return _loadAsync;
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace LCHFramework.Components
{
    public class SceneAssetLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        [PostProcessScene(-1)]
        public static void OnBuild()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var rootGameObjects in SceneManager.GetSceneAt(i).GetRootGameObjects())
                    foreach (var sceneAssetLoader in rootGameObjects.GetComponentsInChildren<SceneAssetLoader>())
                        if (sceneAssetLoader.scene != null)
                            sceneAssetLoader.sceneName = sceneAssetLoader.scene.name;
        }
        
        
        
        public SceneAsset scene;
#endif
        private string sceneName;
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!EditorApplication.isCompiling) sceneName = scene != null ? scene.name : string.Empty;
        }
#endif
        
        
        
        // UnityEvent event.
        public void OnClick() => LoadAsync();



        private AsyncOperation _loadAsync;
        public AsyncOperation LoadAsync()
        {
            if (_loadAsync == null)
            {
                _loadAsync = SceneManager.LoadSceneAsync(sceneName);
                _loadAsync.completed += _ => LCHFramework.DelayFrame(1, () => _loadAsync = null);
            }

            return _loadAsync;
        }
    }
}
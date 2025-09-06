using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Components
{
    public class SceneLoader : MonoBehaviour, ILoader
    {
        [SerializeField] private bool loadOnStart = true;
#if UNITY_EDITOR
        public SceneAsset scene;
#endif
        
        
        [HideInInspector] [SerializeField] private string sceneName;
        
        
#if UNITY_EDITOR
        private void OnValidate() => sceneName = scene != null ? scene.name : string.Empty;
#endif
        private void Start()
        {
            if (loadOnStart) _ = LoadAsync();
        }  
        
        
        
        // UnityEvent event.
        public void OnClick() => LoadAsync();
        
        
        
        public AsyncOperation LoadAsync() => SceneManager.LoadSceneAsync(sceneName);
    }
}
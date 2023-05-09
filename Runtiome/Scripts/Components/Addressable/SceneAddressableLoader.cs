#if Addressable
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LCHFramework.Components.Addressable
{
    public class SceneAddressableLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        [PostProcessScene(-1)]
        public static void OnBuild()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var rootGameObjects in SceneManager.GetSceneAt(i).GetRootGameObjects())
                foreach (var sceneAddressableLoader in rootGameObjects.GetComponentsInChildren<SceneAddressableLoader>())
                    if (sceneAddressableLoader.scene != null)
                        sceneAddressableLoader.sceneAddressableKey = (string)sceneAddressableLoader.scene.RuntimeKey;
        }
        
        

        public AssetReferenceT<SceneAsset> scene;
#endif
        
        private string sceneAddressableKey;
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!EditorApplication.isCompiling) sceneAddressableKey = scene != null ? (string)scene.RuntimeKey : string.Empty;
        }
#endif
        
        
        
        // UnityEvent event.
        public void OnClick() => LoadAsync();



        private AsyncOperationHandle<SceneInstance> _loadAsync;
        public AsyncOperationHandle<SceneInstance> LoadAsync()
        {
            if (_loadAsync.IsDone)
            {
                _loadAsync = Addressables.LoadSceneAsync(sceneAddressableKey);
            }

            return _loadAsync;
        }
    }
}
#endif

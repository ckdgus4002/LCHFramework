using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using LCHFramework.Components;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Addressables.Components
{
#if UNITY_EDITOR
    public class AddressableSceneLoader : AddressableLoader<SceneAsset, SceneInstance>
#else
    public class AddressableSceneLoader : AddressableLoader<Object, SceneInstance>
#endif
    {
        [SerializeField] private LoadSceneMode loadSceneMode;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private string message;
        
        
        
        protected override AsyncOperationHandle<SceneInstance> OnLoadAsync()
        {
            return SceneManager.LoadSceneAsync(address, loadSceneMode, fadeOutDuration, fadeInDuration, message).Result;
        }
    }
}
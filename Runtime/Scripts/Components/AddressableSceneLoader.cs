using System.Threading.Tasks;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Managers;
using LCHFramework.Managers.UI;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Components
{
#if UNITY_EDITOR
    public class AddressableSceneLoader : AddressableLoader<SceneAsset, SceneInstance>
#else
    public class AddressableSceneLoader : AddressableLoader<Object, SceneInstance>
#endif
    {
        [SerializeField] private LoadSceneMode loadSceneMode;
        
        [ShowIf(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeInDuration;
        
        [ShowIf(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeOutDuration;
        
        [ShowIf(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private string message = Loading.DefaultLoadingMessage;
        
        
        
        public override Task LoadAsync()
            => SceneManager.LoadSceneAsync(address, loadSceneMode, fadeOutDuration, fadeInDuration, message);
    }
}
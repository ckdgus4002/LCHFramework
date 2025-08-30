using System.Threading.Tasks;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Managers;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Components
{
#if UNITY_EDITOR
    public class AddressableSceneLoader : AddressableLoader<SceneAsset>
#else
    public class AddressableSceneLoader : AddressableLoader<Object>
#endif
    {
        [SerializeField] private LoadSceneMode loadSceneMode;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeOutDuration = 0.5f;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeInDuration = 0.5f;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private string message = "Loading...";
        
        
        
        public override Task LoadAsync()
            => SceneManager.LoadSceneAsync(address, loadSceneMode, fadeOutDuration, fadeInDuration, message);
    }
}
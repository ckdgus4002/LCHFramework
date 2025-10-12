using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;

namespace LCHFramework.Components
{
    public class AddressableSceneLoader : AddressableLoader<Object>
    {
        [SerializeField] private LoadSceneMode loadSceneMode;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeOutDuration = 0.5f;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeInDuration = 0.5f;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private string loadingMessage = "Loading...";
        
        [SerializeField] private string message = "";
        
        
        
        private void Start()
        {
            if (LoadOnStart) _ = LoadAsync();
        }
        
        
        
        // UnityEvent event.
        public override void OnClick() => _ = LoadAsync();
        
        
        
        public Awaitable LoadAsync() => SceneManager.LoadSceneAsync(asset.GetAddress(), loadSceneMode, fadeOutDuration, fadeInDuration, loadingMessage, message);
    }
}
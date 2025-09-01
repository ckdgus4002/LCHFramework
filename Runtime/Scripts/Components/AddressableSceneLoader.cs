using System.Threading.Tasks;
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
        [SerializeField] private string message = "Loading...";
        
        
        
        private void Start()
        {
            if (loadOnStart) _ = LoadAsync();
        }
        
        
        
        // UnityEvent event.
        public void OnClick() => _ = LoadAsync();
        
        
        
        public Task LoadAsync() => SceneManager.LoadSceneAsync(asset.GetAddress(), loadSceneMode, fadeOutDuration, fadeInDuration, message);
    }
}
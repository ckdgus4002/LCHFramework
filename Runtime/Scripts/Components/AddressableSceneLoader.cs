using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LCHFramework.Components
{
    public class AddressableSceneLoader : AddressableLoader<Object>
    {
        [SerializeField] private AssetLabelReference addressLabel;
        [SerializeField] private string[] atlasAddresses;
        [SerializeField] private LoadSceneMode loadSceneMode;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeOutDuration = 0.5f;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private float fadeInDuration = 0.5f;
        
        [ShowInInspector(nameof(loadSceneMode), ComparisonOperator.NotEquals, LoadSceneMode.None)]
        [SerializeField] private string loadingMessage = "";
        
        [SerializeField] private bool isUpdateCatalogs = true;
        [SerializeField] private string message = "";
        
        
        
        private void Start()
        {
            if (loadOnStart) LoadAsync().Forget();
        }
        
        
        
        // UnityEvent event.
        public override void OnClick() => LoadAsync().Forget();
        
        
        
        public Awaitable LoadAsync() => SceneManager.LoadSceneAsync(asset.GetAddress(), addressLabel.labelString, atlasAddresses, loadSceneMode, fadeOutDuration, fadeInDuration, loadingMessage, isUpdateCatalogs, message);
    }
}
using LCHFramework.Components;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    [RequireComponent(typeof(AddressableAssetLoader))]
    public class AddressableAssetLoadStep : Step
    {
        private AddressableAssetLoader AssetLoader => _assetLoader == null ? _assetLoader = GetComponent<AddressableAssetLoader>() : _assetLoader;
        private AddressableAssetLoader _assetLoader;
        
        
        
        public override void Show()
        {
            base.Show();

            AssetLoader.AsyncOperationHandle.Completed += _ =>
            {
                MessageBroker.Default.Publish(new PassCurrentStepMessage());
            };
        }
    }
}
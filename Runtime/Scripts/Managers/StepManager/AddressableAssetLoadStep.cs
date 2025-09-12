using System.Linq;
using LCHFramework.Components;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    [RequireComponent(typeof(AddressableAssetLoader))]
    public class AddressableAssetLoadStep : Step
    {
        private AddressableAssetLoader[] AssetLoaders => _assetLoaders ??= GetComponents<AddressableAssetLoader>();
        private AddressableAssetLoader[] _assetLoaders;
        
        
        
        public override void Show()
        {
            base.Show();

            Observable.EveryUpdate()
                .Where(_ => AssetLoaders.All(t => t.IsLoaded))
                .Take(1)
                .Subscribe(_ => MessageBroker.Default.Publish(new PassCurrentStepMessage()))
                .AddTo(gameObject);
        }
    }
}
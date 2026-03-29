using System.Threading;
using LCHFramework.Components;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LCHFramework.Managers.StepManager
{
    [RequireComponent(typeof(AddressableAssetLoader))]
    public class AddressableAssetLoadStep : Step
    {
        private AddressableAssetLoader[] AssetLoaders => _assetLoaders.IsEmpty() ? _assetLoaders = GetComponents<AddressableAssetLoader>() : _assetLoaders;
        private AddressableAssetLoader[] _assetLoaders;
        
        
        
#if UNITY_EDITOR
        private void OnValidate() => AssetLoaders.ForEach(t => t.loadOnStart = false);
#endif
        
        
        
        protected override async Awaitable StartShowAsync(CancellationToken cancellationToken)
        {
            await base.StartShowAsync(cancellationToken);
            
            var loadAssets = new AsyncOperationHandle<Object>[AssetLoaders.Length];
            AssetLoaders.ForEach((t, i) => loadAssets[i] = t.LoadAsync());
            await loadAssets.ForEachAsync(async loadAsset => await loadAsset.ToAwaitable(cancellationToken).SuppressCancellationThrow());
        }

        protected override void EndShow()
        {
            base.EndShow();
            
            MessageBroker.Default.Publish(new PassCurrentStepMessage());
        }
    }
}
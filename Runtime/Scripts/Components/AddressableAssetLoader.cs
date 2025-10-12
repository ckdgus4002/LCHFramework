using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class AddressableAssetLoader : AddressableLoader<Object>
    {
        [SerializeField] protected bool releaseOnDestroy = true;
        
        
        public bool IsLoaded { get; private set; }
        
        
        
        private void Start()
        {
            if (LoadOnStart) _ = LoadAsync();
        }
        
        
        
        private void OnDestroy()
        {
            if (releaseOnDestroy) Release();
        }
        
        
        
        // UnityEvent event.
        public override void OnClick() => _ = LoadAsync();
        
        
        
        public AsyncOperationHandle<Object> LoadAsync()
        {
            var handle = AddressablesLoadManager<Object>.LoadAssetAsync(asset.GetAddress());
            handle.Completed += t => IsLoaded = t.Status == AsyncOperationStatus.Succeeded;
            return handle;
        }

        private void Release() => AddressablesLoadManager<Object>.ReleaseAsset(asset.GetAddress());
    }
}

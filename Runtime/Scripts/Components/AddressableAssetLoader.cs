using System.Threading.Tasks;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class AddressableAssetLoader : AddressableLoader
    {
        [SerializeField] protected bool releaseOnDestroy = true;
        
        
        public bool IsLoaded { get; private set; }
        
        
        
        private void OnDestroy()
        {
            if (releaseOnDestroy) Release();
        }
        
        
        
        public override Task LoadAsync() => AddressablesLoadManager<Object>.LoadAssetAsync(asset.GetAddress()).Task.ContinueWith(t => IsLoaded = t.IsCompletedSuccessfully);

        private void Release() => AddressablesLoadManager<Object>.ReleaseAsset(asset.GetAddress());
    }
}

using System.Threading.Tasks;
using LCHFramework.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class AddressableAssetLoader : AddressableLoader<Object>
    {
        [SerializeField] protected bool releaseOnDestroy = true;
        
        
        public bool IsLoaded { get; private set; }
        
        
        
        private void OnDestroy()
        {
            if (releaseOnDestroy) Release();
        }
        
        
        
        public override Task LoadAsync()
            => AddressablesLoadManager<Object>.LoadAssetAsync(address).Task.ContinueWith(t => IsLoaded = t.IsCompletedSuccessfully);

        private void Release() => AddressablesLoadManager<Object>.ReleaseAsset(address);
    }
}

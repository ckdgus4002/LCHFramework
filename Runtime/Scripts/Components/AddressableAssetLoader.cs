using System.Threading.Tasks;
using LCHFramework.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Components
{
    public class AddressableAssetLoader : AddressableLoader<Object, Object>
    {
        [SerializeField] protected bool releaseOnDestroy = true;
        
        
        
        private void OnDestroy()
        {
            if (releaseOnDestroy) Release();
        }
        
        
        
        public override Task LoadAsync()
            => AddressablesLoadManager<Object>.LoadAssetAsync(address).Task;

        private void Release()
            => AddressablesLoadManager<Object>.ReleaseAsset(address);
    }
}

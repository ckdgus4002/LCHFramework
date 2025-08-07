using LCHFramework.Managers;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        
        
        
        protected override AsyncOperationHandle<Object> OnLoadAsync()
        {
            return AddressablesLoadManager<Object>.LoadAssetAsync(address);
        }

        private void Release()
        {
            if (!IsLoaded) return;
            
            AddressablesLoadManager<Object>.ReleaseAsset(address);
        }
    }
}

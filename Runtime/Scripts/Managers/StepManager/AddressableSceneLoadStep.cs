using LCHFramework.Components;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    [RequireComponent(typeof(AddressableSceneLoader))]
    public class AddressableSceneLoadStep : Step
    {
        private AddressableSceneLoader AddressableSceneLoader => _addressableSceneLoader == null ? _addressableSceneLoader = GetComponent<AddressableSceneLoader>() : _addressableSceneLoader;
        private AddressableSceneLoader _addressableSceneLoader;
        
        
        
#if UNITY_EDITOR
        private void OnValidate() => AddressableSceneLoader.loadOnStart = false;
#endif
        
        
        
        protected override async Awaitable StartShowAsync()
        {
            await base.StartShowAsync();

            AddressableSceneLoader.LoadAsync().Forget();
        }
    }
}
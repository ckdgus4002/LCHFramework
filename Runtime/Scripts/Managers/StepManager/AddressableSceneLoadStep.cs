using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers.StepManager
{
    [RequireComponent(typeof(AddressableSceneLoader))]
    public class AddressableSceneLoadStep : Step
    {
        private AddressableSceneLoader AddressableSceneLoader => _addressableSceneLoader == null ? _addressableSceneLoader = GetComponent<AddressableSceneLoader>() : _addressableSceneLoader;
        private AddressableSceneLoader _addressableSceneLoader;
        
        
        
        public override void Show()
        {
            base.Show();

            if (!AddressableSceneLoader.LoadOnStart) AddressableSceneLoader.LoadAsync();
        }
    }
}
using LCHFramework.Addressables.Components;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    [RequireComponent(typeof(AddressableSceneLoader))]
    public class AddressableSceneLoadStep : Step
    {
        private AddressableSceneLoader SceneLoader => _sceneLoader == null ? _sceneLoader = GetComponent<AddressableSceneLoader>() : _sceneLoader;
        private AddressableSceneLoader _sceneLoader;
        
        
        
        public override void Show()
        {
            base.Show();
            
            SceneLoader.AsyncOperationHandle.Completed += _ =>
            {
                MessageBroker.Default.Publish(new PassCurrentStepMessage());
            };
        }
    }
}
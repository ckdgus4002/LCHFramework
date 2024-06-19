using System.Threading.Tasks;
using LCHFramework.Components;
using UnityEngine;

namespace LCHFramework.Managers
{
    [RequireComponent(typeof(SceneAssetLoader))]
    public class SceneAssetLoadStep : Step
    {
        private SceneAssetLoader SceneLoader => _sceneLoader == null ? _sceneLoader = GetComponent<SceneAssetLoader>() : _sceneLoader;
        private SceneAssetLoader _sceneLoader;
        
        
        
        public override async void Show()
        {
            base.Show();
            
            await new TaskCompletionSource<AsyncOperation>(SceneLoader.LoadAsync()).Task;
            
            // PassCurrentStep.PassCurrentStep();
        }
    }   
}
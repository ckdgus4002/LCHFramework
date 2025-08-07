using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace LCHFramework.Managers
{
    public enum LoadSceneMode
    {
        None,
        LoadingUI,
    }

    public struct BeforeLoadSceneCompleteMessage
    {
        public string sceneName;
    }

    public struct AfterLoadSceneCompleteMessage
    {
        public string sceneName;
    }
    
    public static class SceneManager
    {
        private const string Loading = "Loading";
        private static bool isLoadingScene;
        
        
        
        public static float DefaultFadeOutDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => UI.Loading.DefaultFadeInTime,
            _ => 0
        };
        
        public static float DefaultFadeInDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => UI.Loading.DefaultFadeOutTime,
            _ => 0
        };
        
        public static string DefaultMessage(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => UI.Loading.Instance.DefaultLoadingMessage,
            _ => string.Empty
        };
        
        public static Task<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(string sceneAddress, LoadSceneMode mode)
            => LoadSceneAsync(sceneAddress, mode, DefaultFadeOutDuration(mode), DefaultFadeInDuration(mode), DefaultMessage(mode));
        
        public static async Task<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(string sceneAddress, LoadSceneMode mode, float fadeOutDuration, float fadeInDuration, string message)
        {
            if (isLoadingScene) return UnityEngine.AddressableAssets.Addressables.ResourceManager.CreateCompletedOperation<SceneInstance>(default, $"{nameof(isLoadingScene)} is True!");
            
            
            
            isLoadingScene = true;
            await UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(Loading).Task;
            
            
            
            Resources.UnloadUnusedAssets();
            GC.Collect();
            var loadScene = UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(sceneAddress);
            
            
            
            var loadSceneError = string.Empty;
            fadeOutDuration = mode == LoadSceneMode.None ? 0 : fadeOutDuration;
            fadeInDuration = mode == LoadSceneMode.None ? 0 : fadeInDuration;
            bool isLoadSceneCompleted;
            var startTime = Time.time;
            if (mode == LoadSceneMode.LoadingUI) await UI.Loading.Instance.LoadAsync(() =>
            {
                if (loadScene.IsValid() && loadScene.OperationException != null) loadSceneError = $"{loadScene.OperationException}";
                else if (loadScene.IsValid() && loadScene.Status == AsyncOperationStatus.Failed) loadSceneError = $"{loadScene.Status}";
                
                return !string.IsNullOrEmpty(loadSceneError) ? $"씬 로드 중 문제가 발생하였습니다. 앱을 재시작해주세요. ({loadSceneError[..Mathf.Min(4, loadSceneError.Length)]})" : message;

            }, fadeOutDuration, fadeInDuration, () => loadScene.PercentComplete, () =>
            {
                isLoadSceneCompleted = !loadScene.IsValid() || !loadScene.Result.Scene.isLoaded || !loadScene.IsDone || Time.time - startTime < 1;
                return !isLoadSceneCompleted;
            });
            
            
            
            var sceneName = loadScene.Result.Scene.name;
            MessageBroker.Default.Publish(new BeforeLoadSceneCompleteMessage { sceneName = sceneName });
            await Task.Delay(TimeSpan.FromSeconds(fadeInDuration));
                        
            
            
            isLoadingScene = false;
            MessageBroker.Default.Publish(new AfterLoadSceneCompleteMessage { sceneName = sceneName });
            return loadScene;
        }
    }
}
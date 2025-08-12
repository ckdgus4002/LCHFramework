using System;
using System.Threading.Tasks;
using LCHFramework.Managers.UI;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LCHFramework.Managers
{
    public enum LoadSceneMode
    {
        None,
        LoadingUI,
    }
    
    public struct LoadSceneFadeOutMessage
    {
        public string prevSceneName;
        public string sceneName;
    }
    
    public struct LoadSceneFadeInMessage
    {
        public string prevSceneName;
        public string sceneName;
    }
    
    public struct LoadSceneCompletedMessage
    {
        public string prevSceneName;
        public string sceneName;
    }
    
    public static class SceneManager
    {
        private const string LoadSceneErrorMessage = "문제가 발생하였습니다. 앱을 재시작해주세요.";
        private static bool isLoadingScene;
        
        
        
        public static float DefaultFadeOutDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultFadeInTime,
            _ => 0
        };
        
        public static float DefaultFadeInDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultFadeOutTime,
            _ => 0
        };
        
        public static string DefaultMessage(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultLoadingMessage,
            _ => string.Empty
        };
        
        public static async Task LoadSceneAsync(string sceneAddress, LoadSceneMode mode)
            => await LoadSceneAsync(sceneAddress, mode, DefaultFadeOutDuration(mode), DefaultFadeInDuration(mode), DefaultMessage(mode));
        
        public static async Task LoadSceneAsync(string sceneAddress, LoadSceneMode mode, float fadeOutDuration, float fadeInDuration, string message)
        {
            if (isLoadingScene) { Debug.Log($"{nameof(isLoadingScene)} is True!"); return; }
            
            
            SoundManager.Instance.StopAll();
            isLoadingScene = true;
            var prevSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            MessageBroker.Default.Publish(new LoadSceneFadeOutMessage { prevSceneName = prevSceneName, sceneName = sceneAddress });
            var loadScene = Addressables.LoadSceneAsync(sceneAddress, activateOnLoad: false);
            var startTime = Time.time;
            if (mode == LoadSceneMode.LoadingUI) _ = Loading.Instance.LoadAsync(
                () => loadScene.IsValid() && loadScene.OperationException != null ? $"{LoadSceneErrorMessage} ({loadScene.OperationException})" 
                    : loadScene.IsValid() && loadScene.Status == AsyncOperationStatus.Failed ? $"{LoadSceneErrorMessage} ({loadScene.Status})" 
                    : message,
                fadeOutDuration,
                fadeInDuration,
                () => Math.Min(Time.time - (startTime + fadeOutDuration), !loadScene.IsValid() ? 0 : loadScene.PercentComplete),
                () => loadScene.IsValid() && loadScene.Result.Scene.isLoaded);
            
            
            
            while (Time.time <= startTime + fadeOutDuration + 1 || !loadScene.IsDone) await Awaitable.NextFrameAsync(); 
            
            
            await loadScene.Result.ActivateAsync();
            
            
            isLoadingScene = false;
            _ = Resources.UnloadUnusedAssets();
            GC.Collect();
            
            
            var sceneName = loadScene.Result.Scene.name;
            MessageBroker.Default.Publish(new LoadSceneFadeInMessage { prevSceneName = prevSceneName, sceneName = sceneName });
            await Awaitable.WaitForSecondsAsync(fadeInDuration);
            
            
            MessageBroker.Default.Publish(new LoadSceneCompletedMessage { prevSceneName = prevSceneName, sceneName = sceneName });
        }
    }
}
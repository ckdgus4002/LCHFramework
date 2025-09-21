using System;
using LCHFramework.Extensions;
using LCHFramework.Managers.UI;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace LCHFramework.Managers
{
    public enum LoadSceneMode
    {
        None,
        LoadingUI,
    }
    
    public static class SceneManager
    {
        private const string LoadSceneErrorMessage = "문제가 발생하였습니다. 앱을 재시작해주세요.";
        
        
        
        private static bool isLoadingScene;
        private static bool isUILoadingIsDone;
        private static AsyncOperationHandle<SceneInstance> loadScene;
        
        
        public static string PrevSceneAddress { get; private set; } 
        public static string Message { get; private set; }
        
        
        
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
        
        public static string DefaultLoadingMessage(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultLoadingMessage,
            _ => string.Empty
        };
        
        public static async Awaitable LoadSceneAsync(string sceneAddress, LoadSceneMode mode, string message = "")
            => await LoadSceneAsync(sceneAddress, mode, DefaultFadeOutDuration(mode), DefaultFadeInDuration(mode), DefaultLoadingMessage(mode), message);
        
        public static async Awaitable LoadSceneAsync(string sceneAddress, LoadSceneMode mode, float fadeOutDuration, float fadeInDuration, string loadingMessage, string message = "")
        {
            if (isLoadingScene) { Debug.Log($"{nameof(isLoadingScene)} is True!"); return; }
            
            
            SoundManager.Instance.StopAll();
            isLoadingScene = true;
            Message = !string.IsNullOrWhiteSpace(message) ? message : Message;
            MessageBroker.Default.Publish(new LoadSceneFadeOutMessage { sceneAddress = PrevSceneAddress, nextSceneAddress = sceneAddress });
            
            
            isUILoadingIsDone = false;
            loadScene = default;
            var startTime = Time.time;
            if (mode == LoadSceneMode.LoadingUI) _ = Loading.Instance.LoadAsync(
                () => loadScene.IsValid() && loadScene.OperationException != null ? $"{LoadSceneErrorMessage} ({loadScene.OperationException})"
                    : loadScene.IsValid() && loadScene.Status == AsyncOperationStatus.Failed ? $"{LoadSceneErrorMessage} Status is Failed."
                    : loadingMessage,
                fadeOutDuration,
                fadeInDuration,
                () => Math.Min(Time.time - (startTime + fadeOutDuration), !loadScene.IsValid() ? 0 : loadScene.PercentComplete),
                () => isUILoadingIsDone);
            await Awaitable.WaitForSecondsAsync(fadeOutDuration);
            
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("TempScene");
            
            
            PrevSceneAddress = sceneAddress;
            await (loadScene = Addressables.LoadSceneAsync(sceneAddress)).ToAwaitable();
            
            
            SoundManager.Instance.ClearAll();
            GC.Collect();
            await MessageBroker.Default.Receive<LoadSceneFadeInMessage>().Where(t => t.sceneAddress == sceneAddress).First();
            
            
            await Awaitable.WaitForSecondsAsync(startTime + fadeOutDuration + 1 - Time.time);
            
            
            isUILoadingIsDone = true;
            await Awaitable.WaitForSecondsAsync(fadeInDuration);
            
            
            isLoadingScene = false;
            MessageBroker.Default.Publish(new LoadSceneCompletedMessage { prevSceneAddress = PrevSceneAddress, sceneAddress = sceneAddress });
        }
        
        
        
        public struct LoadSceneFadeOutMessage { public string sceneAddress; public string nextSceneAddress; }
    
        public struct LoadSceneFadeInMessage { public string sceneAddress; }
    
        public struct LoadSceneCompletedMessage { public string prevSceneAddress; public string sceneAddress; }
    }
}
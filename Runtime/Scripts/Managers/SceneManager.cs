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
    public interface ILoadSceneUI
    {
        public Awaitable OnLoadSceneAsync(Func<string> getMessage, float fadeInDuration, float fadeOutDuration, Func<float> getPercentOrNull, Func<bool> getIsDone);
    }
    
    public enum LoadSceneMode
    {
        None,
        LoadingUI,
        ScreenFadeUI,
    }
    
    public static class SceneManager
    {
        private const string LoadSceneErrorMessage = "문제가 발생하였습니다. 앱을 재시작해주세요.";
        
        
        
        private static bool isLoadingScene;
        private static bool uiIsDone;
        private static AsyncOperationHandle<SceneInstance> loadScene;
        
        
        public static string SceneAddress { get; private set; } = "";
        public static string PrevSceneAddress { get; private set; } = "";
        public static string Message { get; private set; } = "";
        
        
        
        public static float DefaultFadeOutDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultFadeInTime,
            LoadSceneMode.ScreenFadeUI => ScreenFader.DefaultFadeInTime,
            _ => 0
        };
        
        public static float DefaultFadeInDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultFadeOutTime,
            LoadSceneMode.ScreenFadeUI => ScreenFader.DefaultFadeOutTime,
            _ => 0
        };
        
        public static string DefaultLoadingMessage(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultLoadingMessage,
            LoadSceneMode.ScreenFadeUI => ScreenFader.DefaultFadeMessage,
            _ => ""
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
            
            
            uiIsDone = false;
            loadScene = default;
            var startTime = Time.time;
            var loadSceneUIorNull = (ILoadSceneUI)(mode switch
            {
                LoadSceneMode.LoadingUI => Loading.Instance,
                LoadSceneMode.ScreenFadeUI => ScreenFader.Instance,
                _ => null
            });
            if (loadSceneUIorNull != null) _ = loadSceneUIorNull.OnLoadSceneAsync(
                () => loadScene.IsValid() && loadScene.OperationException != null ? $"{LoadSceneErrorMessage} ({loadScene.OperationException})"
                    : loadScene.IsValid() && loadScene.Status == AsyncOperationStatus.Failed ? $"{LoadSceneErrorMessage} Status is Failed."
                    : loadingMessage,
                fadeOutDuration,
                fadeInDuration,
                () => Math.Min(Time.time - (startTime + fadeOutDuration), !loadScene.IsValid() ? 0 : loadScene.PercentComplete),
                () => uiIsDone);
            await Awaitable.WaitForSecondsAsync(fadeOutDuration);
            
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("TempScene");
            
            
            PrevSceneAddress = SceneAddress;
            SceneAddress = sceneAddress;
            await (loadScene = Addressables.LoadSceneAsync(sceneAddress)).ToAwaitable();
            
            
            SoundManager.Instance.ClearAll();
            GC.Collect();
            await MessageBroker.Default.Receive<LoadSceneFadeInMessage>().Where(t => t.sceneAddress == sceneAddress).First();
            
            
            await Awaitable.WaitForSecondsAsync(startTime + fadeOutDuration + 1 - Time.time);
            
            
            uiIsDone = true;
            await Awaitable.WaitForSecondsAsync(fadeInDuration);
            
            
            isLoadingScene = false;
            MessageBroker.Default.Publish(new LoadSceneCompletedMessage { prevSceneAddress = PrevSceneAddress, sceneAddress = sceneAddress });
        }
        
        
        
        public struct LoadSceneFadeOutMessage { public string sceneAddress; public string nextSceneAddress; }
    
        public struct LoadSceneFadeInMessage { public string sceneAddress; }
    
        public struct LoadSceneCompletedMessage { public string prevSceneAddress; public string sceneAddress; }
    }
}
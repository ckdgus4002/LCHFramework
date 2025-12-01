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
        public Awaitable LoadAsync(Func<string> getMessage, float fadeInDuration, float fadeOutDuration, Func<float> getPercentOrNull, Func<bool> getIsDone);
    }
    
    public enum LoadSceneMode
    {
        None,
        LoadingUI,
        ScreenFadeUI,
    }
    
    public static class SceneManager
    {
        private const string ErrorMessage = "문제가 발생하였습니다. 앱을 재시작해주세요.";
        
        
        
        private static bool isLoadingScene;
        private static bool uiIsDone;
        private static bool isLoadInProcessing;
        private static AsyncOperationHandle downloadAddressable;
        private static AsyncOperationHandle<SceneInstance> loadScene;
        
        
        public static string SceneAddress { get; private set; } = "";
        public static string PrevSceneAddress { get; private set; } = "";
        public static string Message { get; private set; } = "";
        
        
        
        public static float DefaultFadeOutDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultFadeInDuration,
            LoadSceneMode.ScreenFadeUI => ScreenFader.DefaultFadeInDuration,
            _ => 0
        };
        
        public static float DefaultFadeInDuration(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultFadeOutDuration,
            LoadSceneMode.ScreenFadeUI => ScreenFader.DefaultFadeOutDuration,
            _ => 0
        };
        
        public static string DefaultLoadingMessage(LoadSceneMode mode) => mode switch
        {
            LoadSceneMode.LoadingUI => Loading.DefaultLoadingMessage,
            LoadSceneMode.ScreenFadeUI => ScreenFader.DefaultFadeMessage,
            _ => ""
        };
        
        public static async Awaitable LoadSceneAsync(string sceneAddress, string addressLabel, LoadSceneMode mode, string message = "")
            => await LoadSceneAsync(sceneAddress, addressLabel, mode, DefaultFadeOutDuration(mode), DefaultFadeInDuration(mode), DefaultLoadingMessage(mode), message);
        
        public static async Awaitable LoadSceneAsync(string sceneAddress, string addressLabel, LoadSceneMode mode, float fadeOutDuration, float fadeInDuration, string loadingMessage, string message = "")
        {
            if (isLoadingScene) { Debug.Log($"{nameof(isLoadingScene)} is True!"); return; }
            
            
            SoundManager.Instance.StopAll();
            isLoadingScene = true;
            Message = !string.IsNullOrWhiteSpace(message) ? message : Message;
            MessageBroker.Default.Publish(new LoadSceneFadeOutMessage { sceneAddress = PrevSceneAddress, nextSceneAddress = sceneAddress });


            downloadAddressable = default;
            uiIsDone = false;
            isLoadInProcessing = false;
            var startTime = Time.time;
            var loadSceneUIorNull = (ILoadSceneUI)(mode switch
            {
                LoadSceneMode.LoadingUI => Loading.Instance,
                LoadSceneMode.ScreenFadeUI => ScreenFader.Instance,
                _ => null
            });
            if (loadSceneUIorNull != null) _ = loadSceneUIorNull.LoadAsync(
                () =>
                {
                    var isValid = (!isLoadInProcessing ? downloadAddressable : loadScene).IsValid();
                    var operationException = (!isLoadInProcessing ? downloadAddressable : loadScene).OperationException;
                    var status = (!isLoadInProcessing ? downloadAddressable : loadScene).Status;
                    return isValid && operationException != null ? $"{ErrorMessage} ({operationException})"
                        : isValid && status == AsyncOperationStatus.Failed ? $"{ErrorMessage} Status is Failed."
                        : loadingMessage;
                },
                fadeOutDuration,
                fadeInDuration,
                () =>
                {
                    var isValid = (!isLoadInProcessing ? downloadAddressable : loadScene).IsValid();
                    var percentComplete = (!isLoadInProcessing ? downloadAddressable : loadScene).PercentComplete;
                    return Math.Min(Time.time - (startTime + fadeOutDuration), !isValid ? 0 : percentComplete);
                },
                () => uiIsDone);
            await Awaitable.WaitForSecondsAsync(fadeOutDuration);
            
            
            await AddressablesManager.DownloadAsync(addressLabel, null, null, (_, download) => downloadAddressable = download);


            isLoadInProcessing = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoadScene");
            
            
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
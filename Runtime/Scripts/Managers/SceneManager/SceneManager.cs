using System;
using System.Threading.Tasks;
using LCHFramework.Extensions;
using LCHFramework.Managers.UI;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Debug = UnityEngine.Debug;

namespace LCHFramework.Managers
{
    public enum LoadSceneMode
    {
        None,
        LoadingUI,
    }
    
    public struct LoadSceneFadeOutMessage
    {
        public string sceneName;
    }
    
    public struct LoadSceneFadeInMessage
    {
        public string sceneName;
    }
    
    public struct LoadSceneCompletedMessage
    {
        public string sceneName;
    }
    
    public static class SceneManager
    {
        private const string LoadSceneErrorMessage = "문제가 발생하였습니다. 앱을 재시작해주세요.";
        
        
        
        private static bool isLoadingScene;
        private static bool isUILoadingIsDone;
        private static AsyncOperationHandle<SceneInstance> prevLoadScene;
        private static AsyncOperationHandle<SceneInstance> loadScene;
        
        
        
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
            
            
            MessageBroker.Default.Publish(new LoadSceneFadeOutMessage { sceneName = sceneAddress });
            SoundManager.Instance.StopAll();
            isLoadingScene = true;
            prevLoadScene = loadScene;
            isUILoadingIsDone = false;
            var startTime = Time.time;
            if (mode == LoadSceneMode.LoadingUI) _ = Loading.Instance.LoadAsync(
                () => loadScene.IsValid() && loadScene.OperationException != null ? $"{LoadSceneErrorMessage} ({loadScene.OperationException})"
                    : loadScene.IsValid() && loadScene.Status == AsyncOperationStatus.Failed ? $"{LoadSceneErrorMessage} Status is Failed."
                    : message,
                fadeOutDuration,
                fadeInDuration,
                () => Math.Min(Time.time - (startTime + fadeOutDuration), !loadScene.IsValid() ? 0 : loadScene.PercentComplete),
                () => isUILoadingIsDone);
            await Awaitable.WaitForSecondsAsync(fadeOutDuration);
            
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("TempScene");
            await (loadScene = Addressables.LoadSceneAsync(sceneAddress)).ToAwaitable();
            
            
            SoundManager.Instance.ClearAll();
            GC.Collect();
            var sceneName = loadScene.Result.Scene.name;
            await MessageBroker.Default.Receive<LoadSceneFadeInMessage>().Where(t => t.sceneName == sceneName).Take(1).ToTask();
            
            
            await Awaitable.WaitForSecondsAsync(startTime + fadeOutDuration + 1 - Time.time);
            
            
            isUILoadingIsDone = true;
            await Awaitable.WaitForSecondsAsync(fadeInDuration);
            
            
            MessageBroker.Default.Publish(new LoadSceneCompletedMessage { sceneName = sceneName });
            isLoadingScene = false;
        }
    }
}
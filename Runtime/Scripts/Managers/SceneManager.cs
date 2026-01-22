using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Managers.UI;
using LCHFramework.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.U2D;

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
        private static string GetErrorMessage(params object[] args) => string.Format("{0} (이)가 발생하였습니다. 앱을 재시작해주세요.", args);
        
        
        
        private static bool isLoadingScene;
        private static bool uiIsDone;
        private static int loadSceneProcess;
        private static AsyncOperationHandle<List<IResourceLocator>> updateAddressableCatalogs;
        private static AsyncOperationHandle downloadAddressable;
        private static AsyncOperationHandle<SceneInstance> loadScene;
        
        
        public static string SceneAddress { get; private set; } = "";
        public static string PrevSceneAddress { get; private set; } = "";
        private static string[] PrevAtlasAddresses { get; set; } = Array.Empty<string>();
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
        
        public static string DefaultErrorMessage(int errorCode, string error) => GetErrorMessage($"{error}{errorCode}");
        
        
        
        public static Awaitable LoadSceneAsync(string sceneAddress, string addressLabel, string[] atlasAddresses, LoadSceneMode mode, string message = "")
            => LoadSceneAsync(sceneAddress, addressLabel, atlasAddresses, mode, DefaultFadeOutDuration(mode), DefaultFadeInDuration(mode), DefaultLoadingMessage(mode), message);
        
        public static Awaitable LoadSceneAsync(string sceneAddress, string addressLabel, string[] atlasAddresses, LoadSceneMode mode, float fadeOutDuration, float fadeInDuration, string loadingMessage, string message = "")
            => LoadSceneAsync(sceneAddress, addressLabel, atlasAddresses, mode, fadeOutDuration, fadeInDuration, loadingMessage, DefaultErrorMessage, message);
        
        public static async Awaitable LoadSceneAsync(string sceneAddress, string addressLabel, string[] atlasAddresses, LoadSceneMode mode, float fadeOutDuration, float fadeInDuration, string loadingMessage, Func<int, string, string> getErrorMessage, string message = "")
        {
            var log = $"[{nameof(SceneManager)}] {nameof(LoadSceneAsync)}: {sceneAddress}, {nameof(message)}: {message}";
            if (!isLoadingScene) Debug.Log($"{log}.");
            else { Debug.LogWarning($"{log}, {nameof(isLoadingScene)}: {isLoadingScene}!"); return; }
            
            
            SoundManager.Instance.StopAll();
            isLoadingScene = true;
            uiIsDone = false;
            loadSceneProcess = -1;
            updateAddressableCatalogs = default;
            downloadAddressable = default;
            loadScene = default;
            Message = !string.IsNullOrWhiteSpace(message) ? message : Message;
            
            
            var startTime = Time.time;
            var loadSceneUIorNull = (ILoadSceneUI)(mode switch
            {
                LoadSceneMode.LoadingUI => Loading.Instance,
                LoadSceneMode.ScreenFadeUI => ScreenFader.Instance,
                _ => null
            });
            if (loadSceneUIorNull != null) _ = loadSceneUIorNull.LoadAsync(
                () => {
                    var isValid = (loadSceneProcess < 1 ? updateAddressableCatalogs : loadSceneProcess < 2 ? downloadAddressable : loadScene).IsValid();
                    if (!isValid) return loadingMessage;
                    
                    var operationException = loadSceneProcess < 1 ? $"{updateAddressableCatalogs.OperationException}" : loadSceneProcess < 2 ? AddressablesManager.GetDownloadError(downloadAddressable) : $"{loadScene.OperationException}";
                    var status = (loadSceneProcess < 1 ? updateAddressableCatalogs : loadSceneProcess < 2 ? downloadAddressable : loadScene).Status;
                    var hasOperationException = !string.IsNullOrEmpty(operationException);
                    if (hasOperationException) UnityEngine.Debug.LogError(operationException);
                    return hasOperationException ? getErrorMessage(loadSceneProcess, "오류") : status == AsyncOperationStatus.Failed ? getErrorMessage(loadSceneProcess, "실패") : loadingMessage;
                },
                fadeOutDuration,
                fadeInDuration,
                () => {
                    var isValid = (loadSceneProcess < 1 ? updateAddressableCatalogs : loadSceneProcess < 2 ? downloadAddressable : loadScene).IsValid();
                    if (!isValid) return 0;
                    
                    var percentComplete = (loadSceneProcess < 1 ? updateAddressableCatalogs : loadSceneProcess < 2 ? downloadAddressable : loadScene).PercentComplete;
                    return Math.Min(Time.time - (startTime + fadeOutDuration), percentComplete);
                },
                () => uiIsDone);
            MessageBroker.Default.Publish(new LoadSceneFadeOutMessage { sceneAddress = PrevSceneAddress, nextSceneAddress = sceneAddress });
            await Awaitable.WaitForSecondsAsync(fadeOutDuration);
            
            
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoadScene");
            
            
            SoundManager.Instance.ClearAll();
            PrevAtlasAddresses.ForEach(AddressablesLoadManager<SpriteAtlas>.ReleaseAsset);
            GC.Collect();
            loadSceneProcess = 0;
            await AddressablesManager.UpdateCatalogsAsync(true, 
                null, 
                updateCatalogs => updateAddressableCatalogs = updateCatalogs,
                result => result?.ForEach((t, i) => Debug.Log($"Update Catalogs({i}): {t}. {string.Join(", ", t)}"))
            );
            
            
            loadSceneProcess = 1;
            await AddressablesManager.DownloadAsync(addressLabel,
                null, 
                downloadSizeByte => {
                    Debug.Log($"Download Size: {FileUtility.ToHumanReadableFileSize(downloadSizeByte)}");
                    return AwaitableUtility.FromResult(true);
                },
                download => downloadAddressable = download
            );
            
            
            PrevAtlasAddresses = atlasAddresses;
            await atlasAddresses.Select(AddressablesLoadManager<SpriteAtlas>.LoadAssetAsync).ForEachAsync(async loadAtlas => await loadAtlas.ToAwaitable());
            
            
            loadSceneProcess = 2;
            PrevSceneAddress = SceneAddress;
            SceneAddress = sceneAddress;
            await (loadScene = Addressables.LoadSceneAsync(sceneAddress)).ToAwaitable();
            
            
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
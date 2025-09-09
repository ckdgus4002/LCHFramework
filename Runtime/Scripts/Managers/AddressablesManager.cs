using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using Debug = UnityEngine.Debug;

namespace LCHFramework.Managers
{
    public static class AddressablesManager
    {
        public static async Awaitable<bool> DownloadAsync(string label,
            Action<AsyncOperationHandle<long>> onDownloadSize,
            Func<AsyncOperationHandle<long>, Awaitable<bool>> waitForCanDownload,
            Action<AsyncOperationHandle<long>, AsyncOperationHandle> onDownload,
            float minimumDuration = 1f)
        {
            var downloadIsSuccess = false;
            var downloadSize = Addressables.GetDownloadSizeAsync(label);
            onDownloadSize?.Invoke(downloadSize);
            var downloadSizeStartTime = Time.time;
            await AwaitableUtility.WaitWhile(() => !downloadSize.IsDone || Time.time - downloadSizeStartTime < minimumDuration);
            
            if (downloadSize.Status == AsyncOperationStatus.Succeeded)
            {
                if (downloadSize is { Result: > 0 })
                {
                    var canDownload = waitForCanDownload == null || await waitForCanDownload.Invoke(downloadSize);
                    if (!canDownload) return false;
                
                    var download = DownloadAsync(label, false);
                    onDownload?.Invoke(downloadSize, download);
                    var downloadStartTime = Time.time;
                    await AwaitableUtility.WaitWhile(() => !download.IsDone || Time.time - downloadStartTime < minimumDuration);
                    
                    downloadIsSuccess = download.Status == AsyncOperationStatus.Succeeded;
                    Addressables.Release(download);
                }
                else
                    downloadIsSuccess = true;
            }
            Addressables.Release(downloadSize);   

            return downloadIsSuccess;
        }
        
        public static AsyncOperationHandle DownloadAsync(string label, bool autoReleaseHandle = true)
        {
            var operationHandle = Addressables.DownloadDependenciesAsync(label, autoReleaseHandle);
            operationHandle.Completed += handle =>
            {
                var dlError = GetDownloadError(handle);
                if (!string.IsNullOrEmpty(dlError))
                {
                    // handle what error
                    Debug.LogError(dlError);
                }
            };
            return operationHandle;
        }
        
        // https://docs.unity3d.com/Packages/com.unity.addressables@2.6/manual/LoadingAssetBundles.html
        public static string GetDownloadError(AsyncOperationHandle fromHandle)
        {
            if (fromHandle.Status != AsyncOperationStatus.Failed)
                return null;

            RemoteProviderException remoteException;
            Exception e = fromHandle.OperationException;
            while (e != null)
            {
                remoteException = e as RemoteProviderException;
                if (remoteException != null)
                    return remoteException.WebRequestResult.Error;
                e = e.InnerException;
            }

            return null;
        }
        
        
        
        public static List<string> GetAddresses<T>(string label)
        {
            var result = new List<string>();
            foreach (var locator in Addressables.ResourceLocators)
                if (locator.Locate(label, typeof(T), out var locations))
                    result.AddRange(locations.Select(t => t.PrimaryKey));

            return result;
        }
    }
}
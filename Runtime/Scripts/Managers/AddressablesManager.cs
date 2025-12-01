using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

namespace LCHFramework.Managers
{
    public static class AddressablesManager
    {
        public static async Awaitable<bool> DownloadAsync(string label,
            Action<AsyncOperationHandle<long>> onDownloadSize,
            Func<AsyncOperationHandle<long>, Awaitable<bool>> getCanDownload,
            Action<AsyncOperationHandle<long>, AsyncOperationHandle> onDownload)
        {
            var downloadIsSuccess = false;
            var downloadSize = Addressables.GetDownloadSizeAsync(label);
            onDownloadSize?.Invoke(downloadSize);
            await AwaitableUtility.WaitWhile(() => !downloadSize.IsDone);
            
            if (downloadSize.Status == AsyncOperationStatus.Succeeded)
            {
                if (downloadSize is { Result: > 0 })
                {
                    var canDownload = getCanDownload == null || await getCanDownload.Invoke(downloadSize);
                    if (!canDownload) return false;
                
                    var download = DownloadAsync(label, false);
                    onDownload?.Invoke(downloadSize, download);
                    await AwaitableUtility.WaitWhile(() => !download.IsDone);
                    
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
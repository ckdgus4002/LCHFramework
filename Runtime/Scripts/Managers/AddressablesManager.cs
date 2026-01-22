using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

namespace LCHFramework.Managers
{
    public static class AddressablesManager
    {
        private static bool CanRemoteProcess => !IsRemoteCatalog || NetworkReachability.NotReachable != UnityEngine.Application.internetReachability;
        
        private static bool IsRemoteCatalog => _isRemoteCatalog ??= Addressables.ResourceLocators.Any(t => t.LocatorId.StartsWith("http")); 
        private static bool? _isRemoteCatalog;
        
        
        
        public static async Awaitable<bool> UpdateCatalogsAsync(bool autoCleanBundleCache = false,
            Action<AsyncOperationHandle<List<string>>> onCheckForCatalogUpdates = null,
            Action<AsyncOperationHandle<List<IResourceLocator>>> onUpdateCatalogs = null,
            Action<List<IResourceLocator>> onEndUpdateCatalogs = null)
        {
            var canCheckForCatalogUpdates = CanRemoteProcess;
            if (!canCheckForCatalogUpdates) return false;
            
            var checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
            onCheckForCatalogUpdates?.Invoke(checkForCatalogUpdates);
            await checkForCatalogUpdates.ToAwaitable();

            var result = false;
            if (checkForCatalogUpdates.Status == AsyncOperationStatus.Succeeded)
            {
                var catalogs = checkForCatalogUpdates.Result;
                if (catalogs.Exists())
                {
                    await AwaitableUtility.WaitUntil(() => !autoCleanBundleCache || Caching.ready);
                    
                    var canUpdateCatalogs = CanRemoteProcess;
                    if (canUpdateCatalogs)
                    {
                        var updateCatalogs = Addressables.UpdateCatalogs(autoCleanBundleCache, catalogs, false);
                        onUpdateCatalogs?.Invoke(updateCatalogs);
                        var catalogLocators = await updateCatalogs.ToAwaitable();

                        onEndUpdateCatalogs?.Invoke(catalogLocators);
                        result = updateCatalogs.Status == AsyncOperationStatus.Succeeded;
                        Addressables.Release(updateCatalogs);
                    }
                }
                else
                    result = true;
            }
            Addressables.Release(checkForCatalogUpdates);

            return result;
        }
        
        public static async Awaitable<bool> DownloadAsync(string label,
            Action<AsyncOperationHandle<long>> onDownloadSize = null,
            Func<long, Awaitable<bool>> getCanDownload = null,
            Action<AsyncOperationHandle> onDownload = null)
        {
            var canDownloadSize = CanRemoteProcess;
            if (!canDownloadSize) return false;
            
            var downloadSize = Addressables.GetDownloadSizeAsync(label);
            onDownloadSize?.Invoke(downloadSize);
            await downloadSize.ToAwaitable();
            
            var result = false;
            if (downloadSize.Status == AsyncOperationStatus.Succeeded)
            {
                var downloadSizeByte = downloadSize.Result;
                if (downloadSizeByte > 0)
                {
                    var canDownload = CanRemoteProcess && (getCanDownload == null || await getCanDownload.Invoke(downloadSizeByte));
                    if (canDownload)
                    {
                        var download = DownloadAsync(label, false);
                        onDownload?.Invoke(download);
                        await download.ToAwaitable();
                    
                        result = download.Status == AsyncOperationStatus.Succeeded;
                        Addressables.Release(download);   
                    }
                }
                else
                    result = true;
            }
            Addressables.Release(downloadSize);   

            return result;
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
                    UnityEngine.Debug.LogError(dlError);
                }
            };
            return operationHandle;
        }
        
        // https://docs.unity3d.com/Packages/com.unity.addressables@2.8/manual/LoadingAssetBundles.html#handle-download-errors
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
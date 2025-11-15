using System;
using System.Collections.Generic;
using LCHFramework.Extensions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace LCHFramework.Managers
{
    public static class AddressablesLoadManager<T> where T : Object
    {
        private static readonly Dictionary<string, AsyncOperationHandle<T>> LoadedAssets = new();
        
        
        
        public static AsyncOperationHandle<T> LoadAssetAsync(AssetReferenceT<T> assetReferenceT)
            => LoadAssetAsync(assetReferenceT.GetAddress(), assetReferenceT.LoadAssetAsync);
        
        public static AsyncOperationHandle<T> LoadAssetAsync(string address)
            => LoadAssetAsync(address, () => Addressables.LoadAssetAsync<T>(address));
        
        private static AsyncOperationHandle<T> LoadAssetAsync(string address, Func<AsyncOperationHandle<T>> loadAssetAsync)
        {
            if (!LoadedAssets.ContainsKey(address))
            {
                var m_Handle = loadAssetAsync.Invoke();
                m_Handle.Completed += handle =>
                {
                    string dlError = AddressablesManager.GetDownloadError(m_Handle);
                    if (!string.IsNullOrEmpty(dlError))
                    {
                        // handle what error
                        Debug.LogError(dlError);
                    }
                    else if (handle.Result is SpriteAtlas spriteAtlas)
                    {
                        SpriteAtlasBindingManager.AddSpriteAtlas(spriteAtlas);
                    }
                };
                LoadedAssets.Add(address, m_Handle);
            }
            
            return LoadedAssets[address];
        }
        
        
        
        public static void ReleaseAsset(string address)
        {
            if (LoadedAssets.Remove(address, out var value))
            {
                if (value.Result is SpriteAtlas spriteAtlas) SpriteAtlasBindingManager.RemoveSpriteAtlas(spriteAtlas);
                
                value.Release();
            }
        }
    }
}
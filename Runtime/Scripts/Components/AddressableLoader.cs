using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace LCHFramework.Components
{
    public abstract class AddressableLoader<T1, T2> : LCHMonoBehaviour where T1 : Object
    {
        [SerializeField] private bool loadOnStart = true;

#if UNITY_EDITOR
        [SerializeField] private AssetReferenceT<T1> asset;
#endif  
        
        [HideInInspector] [SerializeField] protected string address;
        
        
        public AsyncOperationHandle<T2> AsyncOperationHandle { get; protected set; }
        
        
        public bool IsLoaded => AsyncOperationHandle.IsValid();
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            address = asset != null ? AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(asset.AssetGUID).address : string.Empty;
        }
#endif  
        
        protected override void Start()
        {
            base.Start();

            if (loadOnStart) _ = LoadAsync();
        }
        
        
        
        // UnityEvent event.
        public void OnClick() => _ = LoadAsync();

        
        
        public Task LoadAsync()
        {
            if (!IsLoaded)
            {
                AsyncOperationHandle = OnLoadAsync();
            }

            return AsyncOperationHandle.Task;
        }

        protected abstract AsyncOperationHandle<T2> OnLoadAsync();
    }
}
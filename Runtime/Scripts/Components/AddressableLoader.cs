using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace LCHFramework.Components
{
    public abstract class AddressableLoader<T> : MonoBehaviour where T : Object
    {
        [SerializeField] private bool loadOnStart = true;

#if UNITY_EDITOR
        [SerializeField] private AssetReferenceT<T> asset;
#endif  
        
        [HideInInspector] [SerializeField] protected string address;
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            address = asset == null ? string.Empty : AddressableAssetSettingsDefaultObject.Settings?.FindAssetEntry(asset.AssetGUID)?.address;
        }
#endif  
        
        private void Start()
        {
            if (loadOnStart) _ = LoadAsync();
        }
        
        
        
        // UnityEvent event.
        public void OnClick() => _ = LoadAsync();

        
        
        public abstract Task LoadAsync();
    }
}
using System.Threading.Tasks;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

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
        private void OnValidate() => address = asset == null ? string.Empty : asset.GetAddress();
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
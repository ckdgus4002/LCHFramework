using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LCHFramework.Components
{
    public abstract class AddressableLoader : MonoBehaviour
    {
        [SerializeField] private bool loadOnStart = true;
        [SerializeField] protected AssetReference asset;
        
        
        
        private void Start()
        {
            if (loadOnStart) _ = LoadAsync();
        }
        
        
        
        // UnityEvent event.
        public void OnClick() => _ = LoadAsync();

        
        
        public abstract Task LoadAsync();
    }
}
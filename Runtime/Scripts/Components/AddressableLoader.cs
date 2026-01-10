using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LCHFramework.Components
{
    public abstract class AddressableLoader<T> : MonoBehaviour, ILoader where T : Object 
    {
        public bool loadOnStart;
        [SerializeField] protected AssetReferenceT<T> asset;
        
        
        
        public abstract void OnClick();
    }
}
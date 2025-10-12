using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LCHFramework.Components
{
    public abstract class AddressableLoader<T> : MonoBehaviour, ILoader where T : Object 
    {
        [SerializeField] protected AssetReferenceT<T> asset;
        
        
        [field: SerializeField] public bool LoadOnStart { get; private set; }
        
        
        
        public abstract void OnClick();
    }
}
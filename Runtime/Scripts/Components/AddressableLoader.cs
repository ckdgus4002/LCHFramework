using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LCHFramework.Components
{
    public abstract class AddressableLoader<T> : MonoBehaviour where T : Object
    {
        [SerializeField] protected bool loadOnStart = true;
        [SerializeField] protected AssetReferenceT<T> asset;
    }
}
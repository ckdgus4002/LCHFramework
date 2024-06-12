using UnityEngine;

namespace LCHFramework.Managers
{
    public abstract class RuntimeScriptableSingleton<T> : ScriptableObject
    {
        public static T Instance { get; protected set; }



        /// <remarks>
        /// ex. Instance = Resources.Load();
        /// </remarks>>
        protected abstract void InitializeInstance();
    }
}

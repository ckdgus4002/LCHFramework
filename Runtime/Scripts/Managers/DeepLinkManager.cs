using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers
{
    public abstract class DeepLinkManager<T> : MonoSingleton<T> where T : DeepLinkManager<T>
    {
        private const string HasDeferredDeepLinkPrefsKey = "HasDeferredDeepLink";
        
        
        
        [SerializeField] protected string[] queryKeys;
        
        
        public Dictionary<string, bool> IsActivatingDeepLink { get; private set; } = new();
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            IsActivatingDeepLink = queryKeys.ToDictionary(t => t, _ => false);
        }
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            UnityEngine.Application.deepLinkActivated += OnDeepLinkActivated;
            
            var getDeferredDeepLink = (false, string.Empty);
            if (PlayerPrefs.GetInt(HasDeferredDeepLinkPrefsKey, 0) == 0)
            {
                yield return GetDeferredDeepLink(result => getDeferredDeepLink = result);
                
                PlayerPrefsUtility.SetInt(HasDeferredDeepLinkPrefsKey, 1);
            }
            
            var absoluteURL = UnityEngine.Application.absoluteURL;
            if (!string.IsNullOrEmpty(absoluteURL))
                OnDeepLinkActivated(absoluteURL);
            else if (!string.IsNullOrEmpty(getDeferredDeepLink.Item2))
                OnDeepLinkActivated($"{getDeferredDeepLink.Item2}");
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            UnityEngine.Application.deepLinkActivated -= OnDeepLinkActivated;
        }
        
        
        
        private void OnDeepLinkActivated(string url)
        {
            var urls = url.Split('?');
            var queries = urls.Length < 2 ? Array.Empty<string[]>() : urls[1].Split('&').Select(t => t.Split('=')).Where(t => t.Length == 2).ToArray();
            for (var queryIndex = 0; queryIndex < queries.Length; queryIndex++)
                foreach (var _ in queryKeys.Where(t => string.Equals(queries[queryIndex][0], t, StringComparison.OrdinalIgnoreCase)))
                {
                    var queryKey = queries[queryIndex][0];
                    var queryValue = queries[queryIndex][1];
                    var log = $"[DeepLinkManager/{nameof(OnDeepLinkActivated)}] {nameof(url)}: {url}, {nameof(queries)}Length: {queries.Length}, {nameof(queryIndex)}: {queryIndex}, {nameof(queryKey)}: {queryKey}, {nameof(queryValue)}: {queryValue}.";
                    if (!IsActivatingDeepLink[queryKey]) Debug.Log($"{log}.");
                    else { Debug.LogWarning($"{log}, {nameof(IsActivatingDeepLink)}: {IsActivatingDeepLink[queryKey]}!"); continue; }
                    
                    IsActivatingDeepLink[queryKey] = true;
                    OnDeepLinkActivated(url, queries, queryIndex, queryKey, queryValue).GetAwaiter().OnCompleted(() => IsActivatingDeepLink[queryKey] = false);
                }
        }
        
        protected abstract Awaitable OnDeepLinkActivated(string url, string[][] queries, int queryIndex, string queryKey, string queryValue);
        
        
        
        protected abstract IEnumerator GetDeferredDeepLink(Action<(bool, string)> deferredDeepLink);
    }
}
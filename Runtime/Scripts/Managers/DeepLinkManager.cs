using System;
using System.Collections;
using System.Linq;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Managers
{
    public abstract class DeepLinkManager : MonoSingleton<DeepLinkManager>
    {
        protected const string DeferredDeepLinkPrefsKey = "DeferredDeepLink";
        
        
        
        [SerializeField] protected string [] queryKeys;
        
        
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            UnityEngine.Application.deepLinkActivated += OnDeepLinkActivated;
            
            var getDeferredDeepLink = (false, "");
            if (!PlayerPrefs.HasKey(DeferredDeepLinkPrefsKey))
            {
                yield return GetDeferredDeepLink(result => getDeferredDeepLink = result);
                
                PlayerPrefsUtility.SetInt(DeferredDeepLinkPrefsKey, 1);
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
            Debug.Log($"[{nameof(DeepLinkManager)}/{nameof(OnDeepLinkActivated)}] {nameof(url)}: {url}");
            
            var urls = url.Split('?');
            if (urls.Length < 2) return;
            
            ActiveDeepLink(url, urls[1]);
        }
        
        
        
        protected abstract IEnumerator GetDeferredDeepLink(Action<(bool, string)> deferredDeepLink);
        
        protected void ActiveDeepLink(string url, string query)
        {
            var queries = query.Split('&').Select(t => t.Split('=')).Where(t => t.Length == 2).ToArray();
            for (var i = 0; i < queries.Length; i++)
                foreach (var _ in queryKeys.Where(t => string.Equals(queries[i][0], t, StringComparison.OrdinalIgnoreCase)))
                    OnDeepLinkActivated(url, queries, i, queries[i][0], queries[i][1]);
        }
        
        protected virtual void OnDeepLinkActivated(string url, string[][] queries, int queryIndex, string queryKey, string queryValue)
            => Debug.Log($"[{nameof(DeepLinkManager)}/{nameof(OnDeepLinkActivated)}] {nameof(url)}: {url}, {nameof(queries)}Length: {queries.Length}, {nameof(queryIndex)}: {queryIndex}, {nameof(queryKey)}: {queryKey}, {nameof(queryValue)}: {queryValue}");
    }
}
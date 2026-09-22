using System.Collections;
using System.Linq;
using UnityEngine;

namespace LCHFramework.Managers
{
    public abstract class DeepLinkManager : MonoSingleton<DeepLinkManager>
    {
        [SerializeField] protected string [] queryKeys;
        
        
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            UnityEngine.Application.deepLinkActivated += OnDeepLinkActivated;
            
            var absoluteURL = UnityEngine.Application.absoluteURL;
            if (!string.IsNullOrEmpty(absoluteURL)) OnDeepLinkActivated(absoluteURL);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            UnityEngine.Application.deepLinkActivated -= OnDeepLinkActivated;
        }
        
        
        
        private void OnDeepLinkActivated(string url)
        {
            Debug.Log($"[{nameof(DeepLinkManager)}/{nameof(OnDeepLinkActivated)}] {nameof(url)}: {url}");
            
            // Decode the URL to determine action. 
            // In this example, the application expects a link formatted like this:
            // unitydl://mylink?scene1
            var queries = url.Split('?')[1].Split('&').Select(t => t.Split('=')).Where(t => t.Length != 2).ToArray();
            for (var i = 0; i < queries.Length; i++)
                for (var j = 0; j < queryKeys[j].Length; j++)
                    if (queries[i][0] == queryKeys[j]) 
                        OnDeepLinkActivated(url, queries, i, j);
        }
        
        protected abstract void OnDeepLinkActivated(string url, string[][] queries, int queryIndex, int queryKeyIndex);
    }
}
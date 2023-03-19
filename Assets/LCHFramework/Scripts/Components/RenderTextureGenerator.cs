using System.Collections.Generic;
using LCHFramework.Modules;
using UnityEngine;
using UnityEngine.Events;
using MonoBehaviour = LCHFramework.Modules.MonoBehaviour;

namespace LCHFramework.Components
{
    public abstract class RenderTextureGenerator : MonoBehaviour
    {
        [SerializeField] private bool generateOnAwake = true;
        [SerializeField] private bool generateOnEnable = false;
        [SerializeField] private UnityEvent<RenderTexture> onGenerate;
        
        
        private readonly Dictionary<Vector2, RenderTexture> _recentRenderTextures = new Dictionary<Vector2, RenderTexture>();


        public bool IsGenerated => 0 < _recentRenderTextures.Count;
        
        protected abstract int GetRenderTextureWidth();
        
        protected abstract int GetRenderTextureHeight();


        
        protected override void Awake()
        {
            base.Awake();

            if (generateOnAwake) Generate();
        }
        
        private void OnEnable()
        {
            if (generateOnEnable) Generate();
        }



        protected void Generate()
        {
            var key = new Vector2(GetRenderTextureWidth(), GetRenderTextureHeight());
            if (!_recentRenderTextures.ContainsKey(key))
                _recentRenderTextures.Add(key, new RenderTexture(GetRenderTextureWidth(), GetRenderTextureHeight(), 0));
            
            onGenerate?.Invoke(_recentRenderTextures[key]);
        }
    }
}
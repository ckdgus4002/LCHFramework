using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public abstract class RenderTextureGenerator : LCHMonoBehaviour
    {
        [SerializeField] private bool generateOnAwake = true;
        [SerializeField] private bool generateOnEnable;
        [SerializeField] private UnityEvent<RenderTexture> onGenerate;
        
        
        public Dictionary<string, RenderTexture> RenderTextures { get; }= new();
        
        
        public bool IsGenerated => 0 < RenderTextures.Count;
        
        protected abstract int GetRenderTextureWidth();
        
        protected abstract int GetRenderTextureHeight();

        protected virtual int GetRenderTextureDepth() => 0;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (generateOnAwake) Generate();
        }
        
        private void OnEnable()
        {
            if (generateOnEnable) Generate();
        }



        protected void Generate() => Generate($"{GetRenderTextureWidth()};{GetRenderTextureHeight()};{GetRenderTextureDepth()}");
        
        protected void Generate(string key)
        {
            if (!RenderTextures.ContainsKey(key)) RenderTextures.Add(key, new RenderTexture(GetRenderTextureWidth(), GetRenderTextureHeight(), GetRenderTextureDepth()));
            onGenerate?.Invoke(RenderTextures[key]);
        }
    }
}
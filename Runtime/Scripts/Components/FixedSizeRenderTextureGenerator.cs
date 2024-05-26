using System;
using UnityEngine;

namespace LCHFramework.Components
{
    public class FixedSizeRenderTextureGenerator : RenderTextureGenerator
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Camera[] resizeCameras = Array.Empty<Camera>();
        

        protected override int GetRenderTextureWidth() => width;

        protected override int GetRenderTextureHeight() => height;
        
        
        
        private void OnValidate()
        {
            foreach (var item in resizeCameras) if (item != null) item.orthographicSize = GetRenderTextureHeight() / 2f;
        }
    }
}
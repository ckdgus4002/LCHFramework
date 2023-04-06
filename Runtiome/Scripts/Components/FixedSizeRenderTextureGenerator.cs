using System;
using UnityEngine;

namespace LCHFramework.Components
{
    public class FixedSizeRenderTextureGenerator : RenderTextureGenerator
    {
        [SerializeField] [Range(16, 2400)] protected int width = 2400;
        [SerializeField] [Range(9, 1080)] protected  int height = 1080;
        [SerializeField] private Camera[] resizeCameras = Array.Empty<Camera>();
        

        protected override int GetRenderTextureWidth() => width;

        protected override int GetRenderTextureHeight() => height;
        
        
        
        private void OnValidate()
        {
            foreach (var item in resizeCameras) if (item != null) item.orthographicSize = GetRenderTextureHeight() / 2f;
        }
    }
}
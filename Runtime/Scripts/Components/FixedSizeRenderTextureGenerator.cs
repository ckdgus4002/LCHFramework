using UnityEngine;

namespace LCHFramework.Components
{
    public class FixedSizeRenderTextureGenerator : RenderTextureGenerator
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        
        protected override int GetRenderTextureWidth() => width;

        protected override int GetRenderTextureHeight() => height;
    }
}
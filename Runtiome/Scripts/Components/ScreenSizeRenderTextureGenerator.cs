using UnityEngine;

namespace LCHFramework.Components
{
    public class ScreenSizeRenderTextureGenerator : RenderTextureGenerator
    {
        protected override int GetRenderTextureWidth() => Screen.width;

        protected override int GetRenderTextureHeight() => Screen.height;
        
        
        
        // LCHFramework Event.
        private void OnScreenSizeChanged() => Generate();
    }
}
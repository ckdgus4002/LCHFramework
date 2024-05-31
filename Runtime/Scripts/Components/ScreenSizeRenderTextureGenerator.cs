using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Components
{
    public class ScreenSizeRenderTextureGenerator : RenderTextureGenerator, IScreenSizeChanged
    {
        protected override int GetRenderTextureWidth() => Screen.width;

        protected override int GetRenderTextureHeight() => Screen.height;
        
        
        
        // LCHFramework Event.
        public void OnScreenSizeChanged(Vector2 prev, Vector2 current) => Generate();
    }
}
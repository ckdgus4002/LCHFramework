using UnityEngine;

namespace LCHFramework.Components
{
    public class ScreenSizeRenderTextureGenerator : RenderTextureGenerator
    {
        private Vector2 _prevScreenSize;
        
        
        protected override int GetRenderTextureWidth() => Screen.width;

        protected override int GetRenderTextureHeight() => Screen.height;
        
        
        
        private void Update()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            if (!Mathf.Approximately(_prevScreenSize.x, screenSize.x)  
                || !Mathf.Approximately(_prevScreenSize.y, screenSize.y)
                )
            {
                OnChangedScreenSize();
            }
                
            _prevScreenSize = screenSize;
        }
        
        
        
        private void OnChangedScreenSize() => Generate();
    }
}
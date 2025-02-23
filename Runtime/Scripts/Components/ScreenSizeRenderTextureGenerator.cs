using LCHFramework.Data;
using UniRx;
using UnityEngine;

namespace LCHFramework.Components
{
    public class ScreenSizeRenderTextureGenerator : RenderTextureGenerator
    {
        protected override int GetRenderTextureWidth() => Screen.width;

        protected override int GetRenderTextureHeight() => Screen.height;
        
        
        
        protected override void OnEnable()
        {
            base.OnEnable();

            disposables.Add(MessageBroker.Default.Receive<ScreenSizeChangedMessage>().Subscribe(_ => Generate()));
        }
    }
}
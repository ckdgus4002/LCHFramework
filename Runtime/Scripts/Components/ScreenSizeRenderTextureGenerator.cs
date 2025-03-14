using LCHFramework.Data;
using UniRx;
using UnityEngine;

namespace LCHFramework.Components
{
    public class ScreenSizeRenderTextureGenerator : RenderTextureGenerator
    {
        private readonly CompositeDisposable _disposables = new();
        
        
        
        protected override int GetRenderTextureWidth() => Screen.width;
        
        protected override int GetRenderTextureHeight() => Screen.height;
        
        
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _disposables.Add(MessageBroker.Default.Receive<ScreenSizeChangedMessage>().Subscribe(_ => Generate()));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _disposables.Clear();
        }
    }
}
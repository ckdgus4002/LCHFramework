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
        
        
        
        private void OnEnable() => _disposables.Add(MessageBroker.Default.Receive<ScreenSizeChangedMessage>().Subscribe(_ => Generate()));

        private void OnDisable() => _disposables.Clear();
    }
}
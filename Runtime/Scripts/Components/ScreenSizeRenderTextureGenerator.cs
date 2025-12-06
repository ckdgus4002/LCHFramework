using UniRx;

namespace LCHFramework.Components
{
    public class ScreenSizeRenderTextureGenerator : RenderTextureGenerator
    {
        private readonly CompositeDisposable disposables = new();
        
        
        protected override int GetRenderTextureWidth() => Screen.width;
        
        protected override int GetRenderTextureHeight() => Screen.height;
        
        
        
        private void OnEnable() => disposables.Add(MessageBroker.Default.Receive<OnScreenSizeChangedMessage>().Subscribe(_ => Generate()));
        
        private void OnDisable() => disposables.Clear();
    }
}
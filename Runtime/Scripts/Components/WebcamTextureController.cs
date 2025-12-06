using System;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class WebcamTextureController : MonoBehaviour
    {
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool stopOnDisable = true;
        public WebCamDeviceType webCamDeviceType = WebCamDeviceType.FrontFacing;
        public UnityEvent<WebCamTexture> onPlay;
        
        
        
        public async Awaitable<WebCamTexture> GetWebcamTextureOrNull(bool force = false)
        {
            if ((_webcamTextureOrNull == null || force) && await Application.RequestUserCameraPermissionAsync())
            {
                var webCamDeviceExists = WebCamTexture.devices.TryFirstOrDefault(t => 
                    (webCamDeviceType & WebCamDeviceType.FrontFacing) != 0 && t.isFrontFacing,
                    out var webCamDevice);
                _webcamTextureOrNull = !webCamDeviceExists ? null : new WebCamTexture(webCamDevice.name, Screen.width, Screen.height);
            }
            
            return _webcamTextureOrNull;
        }
        private WebCamTexture _webcamTextureOrNull;
        
        
        
        protected virtual void OnEnable()
        {
            if (playOnEnable) Play().Forget();
        }
        
        protected virtual void Start()
        {
            MessageBroker.Default.Receive<OnScreenSizeChangedMessage>().Subscribe(_ => OnScreenSizeChanged()).AddTo(this);
        }
        
        protected virtual void OnDisable()
        {
            if (stopOnDisable) Stop();
        }
        
        
        
        private void OnScreenSizeChanged() => Play(true).Forget();
        
        
        
        public virtual async Awaitable Play(bool restart = false)
        {
            var webcamTextureOrNull = await GetWebcamTextureOrNull(restart);
            
            webcamTextureOrNull.Play();
            
            onPlay?.Invoke(webcamTextureOrNull);
        }
        
        public virtual void Pause()
        {
            if (_webcamTextureOrNull != null) _webcamTextureOrNull.Pause();
        }
        
        public virtual void Stop()
        {
            if (_webcamTextureOrNull != null) _webcamTextureOrNull.Stop();
        }
        
        
        
        [Flags]
        public enum WebCamDeviceType
        {
            FrontFacing = 1 << 0,
        }
    }
}
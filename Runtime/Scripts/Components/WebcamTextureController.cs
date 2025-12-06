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
            if (force || (_webcamTexture == null && await Application.RequestUserCameraPermissionAsync()))
            {
                var webCamDeviceExists = WebCamTexture.devices.TryFirstOrDefault(t => 
                    (webCamDeviceType & WebCamDeviceType.FrontFacing) != 0 && t.isFrontFacing,
                    out var webCamDevice);
                _webcamTexture = !webCamDeviceExists ? null : new WebCamTexture(webCamDevice.name, Screen.width, Screen.height);
            }
            
            return _webcamTexture;
        }
        private WebCamTexture _webcamTexture;
        
        
        
        private void OnEnable()
        {
            if (playOnEnable) Play().Forget();
        }
        
        protected virtual void Start()
        {
            MessageBroker.Default.Receive<OnScreenSizeChangedMessage>().Subscribe(_ => OnScreenSizeChanged()).AddTo(this);
        }
        
        private void OnDisable()
        {
            if (stopOnDisable) Stop();
        }
        
        
        
        private void OnScreenSizeChanged() => Play(true).Forget();
        
        
        
        public async Awaitable Play(bool restart = false)
        {
            var webcamTextureOrNull = await GetWebcamTextureOrNull(restart);
            
            webcamTextureOrNull.Play();
            
            onPlay?.Invoke(webcamTextureOrNull);
        }
        
        public void Pause()
        {
            if (_webcamTexture != null) _webcamTexture.Pause();
        }
        
        public void Stop()
        {
            if (_webcamTexture != null) _webcamTexture.Stop();
        }
        
        
        
        [Flags]
        public enum WebCamDeviceType
        {
            FrontFacing = 1 << 0,
        }
    }
}
using System;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class WebcamTextureController : MonoBehaviour
    {
        protected const UserAuthorization UserAuthorizationWebCam = UserAuthorization.WebCam;
        
        
        
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool pauseOnDisable;
        [SerializeField] private bool stopOnDisable = true;
        public WebCamDeviceType webCamDeviceType = WebCamDeviceType.FrontFacing;
        public UnityEvent<WebCamTexture> onPlay;
        
        
        
        public virtual async Awaitable<WebCamTexture> GetWebcamTextureOrNull(bool force = false, Func<Awaitable<Application.RequestUserPermissionResult>> onRequestPermissionDeniedAndDontAskAgainOrNull = null)
        {
            if (_webcamTextureOrNull == null || force)
            {
                var requestUserPermissionResult = await Application.RequestUserPermissionAsync(UserAuthorizationWebCam);
                if (onRequestPermissionDeniedAndDontAskAgainOrNull != null && requestUserPermissionResult == Application.RequestUserPermissionResult.DeniedAndDontAskAgain) requestUserPermissionResult = await onRequestPermissionDeniedAndDontAskAgainOrNull.Invoke();
                
                if (Application.RequestUserPermissionResult.Granted <= requestUserPermissionResult)
                {
                    var webCamDeviceExists = WebCamTexture.devices.TryFirstOrDefault(t => 
                            (webCamDeviceType & WebCamDeviceType.FrontFacing) != 0 && t.isFrontFacing,
                        out var webCamDevice);
                    _webcamTextureOrNull = !webCamDeviceExists ? null : new WebCamTexture(webCamDevice.name, Screen.width, Screen.height);
                }
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
            if (pauseOnDisable) Pause();
            if (stopOnDisable) Stop();
        }
        
        
        
        private void OnScreenSizeChanged()
        {
            if (_webcamTextureOrNull != null && _webcamTextureOrNull.isPlaying) Play(true).Forget();
        }
        
        
        
        public virtual async Awaitable Play(bool force = false)
        {
            var webcamTextureOrNull = await GetWebcamTextureOrNull(force);
            
            if (webcamTextureOrNull != null) webcamTextureOrNull.Play();
            
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
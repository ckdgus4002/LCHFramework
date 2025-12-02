using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components
{
    public class WebcamTextureController : MonoBehaviour
    {
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool stopOnDisable = true;
        public WebCamDeviceType webCamDeviceType = WebCamDeviceType.FrontFacing; 
        public List<RawImage> rawImages;
        
        
        public virtual int RequestedWidth => 0;
        public virtual int RequestedHeight => 0;
        public virtual int RequestedFPS => 0;
        
        public async Awaitable<WebCamTexture> GetWebcamTextureOrNull()
        {
            if (_webcamTexture == null && await Application.RequestUserCameraPermissionAsync())
            {
                var webCamDeviceExists = WebCamTexture.devices.TryFirstOrDefault(t => 
                    (webCamDeviceType & WebCamDeviceType.FrontFacing) != 0 && t.isFrontFacing,
                    out var webCamDevice);
                _webcamTexture = !webCamDeviceExists ? null : new WebCamTexture(webCamDevice.name, RequestedWidth, RequestedHeight, RequestedFPS);
            }
            
            return _webcamTexture;
        }
        private WebCamTexture _webcamTexture;
        
        
        
        private void OnEnable()
        {
            if (playOnEnable) Play();
        }
        
        protected virtual void Start()
        {
            OrientationManager.Instance.Orientation.Subscribe(OnOrientationChanged).AddTo(this);
        }
        
        private void OnDisable()
        {
            if (stopOnDisable) Stop();
        }
        
        
        
        private void OnOrientationChanged(Orientation orientation) => rawImages
            .Where(t => t != null)
            .ForEach(t => t.rectTransform.localEulerAngles = t.rectTransform.localEulerAngles.SetZ(orientation != Orientation.LandscapeRight ? 0 : 180));
        
        
        
        public async void Play()
        {
            var webcamTextureOrNull = await GetWebcamTextureOrNull();
            
            rawImages.Where(t => t != null).ForEach(t => t.texture = webcamTextureOrNull);
            if (webcamTextureOrNull != null) webcamTextureOrNull.Play();
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
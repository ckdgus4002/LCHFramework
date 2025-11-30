using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using LCHFramework.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components
{
    public class WebcamTextureController : LCHMonoBehaviour
    {
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool stopOnDisable = true;
        public WebCamDeviceType webCamDeviceType = WebCamDeviceType.FrontFacing; 
        public List<RawImage> rawImages;
        
        
        private CancellationTokenSource _getWebcamTextureCts;
        
        
        public async Awaitable<WebCamTexture> GetWebcamTextureOrNull()
        {
            if (_webcamTexture == null && await RequestUserCameraPermissionAsync())
            {
                var webCamDeviceExists = WebCamTexture.devices.TryFirstOrDefault(t => 
                    (webCamDeviceType & WebCamDeviceType.FrontFacing) != 0 && t.isFrontFacing,
                    out var webCamDevice);
                _webcamTexture = !webCamDeviceExists ? null : new WebCamTexture(webCamDevice.name);
            }
            
            return _webcamTexture;
        }
        private WebCamTexture _webcamTexture;
        
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            rawImages = !gameObject.TryGetComponentsInChildren<RawImage>(true, out var result) ? new List<RawImage>() : result.ToList();
        }
#endif
        protected override void OnEnable()
        {
            base.OnEnable();
            if (playOnEnable) Play();
        }
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            OrientationManager.Instance.Orientation.Subscribe(OnOrientationChanged).AddTo(this);
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (stopOnDisable) Stop();
        }
        
        
        
        private void OnOrientationChanged(Orientation orientation) => rawImages
            .Where(t => t != null)
            .ForEach(t => t.rectTransform.localEulerAngles = t.rectTransform.localEulerAngles.SetZ(orientation != Orientation.LandscapeRight ? 0 : 180));
        
        
        
        public virtual Awaitable<bool> RequestUserCameraPermissionAsync() => AwaitableUtility.FromResult(true);
        
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
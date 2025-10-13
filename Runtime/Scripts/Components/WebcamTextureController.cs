using System;
using System.Collections;
using System.Threading;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components
{
    public abstract class WebcamTextureController : LCHMonoBehaviour
    {
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private bool stopOnDisable = true;
        public WebCamDeviceType webCamDeviceType = WebCamDeviceType.FrontFacing; 
        public RawImage[] rawImages;
        
        
        private CancellationTokenSource _getWebcamTextureCts;
        
        
        private WebCamTexture GetWebcamTextureOrNull(bool canCache)
            => !canCache ? _webcamTexture : throw new NotImplementedException("Instead of using this method, use GetWebcamTextureOrNull()."); 
        
        public async Awaitable<WebCamTexture> GetWebcamTextureOrNull()
        {
            if (_webcamTexture == null)
            {
                await RequestUserCameraPermissionAsync();

                await Awaitable.NextFrameAsync();

                var webCamDeviceExists = WebCamTexture.devices.TryFirstOrDefault(t => (webCamDeviceType & WebCamDeviceType.FrontFacing) != 0 && t.isFrontFacing
                    , out var webCamDevice);
                _webcamTexture = !webCamDeviceExists ? null : new WebCamTexture(webCamDevice.name);
            }
            
            return _webcamTexture;
        }
        private WebCamTexture _webcamTexture;
        
        
        
#if UNITY_EDITOR
        private void Reset()
        {
            rawImages = !gameObject.TryGetComponentsInChildren<RawImage>(true, out var result) ? Array.Empty<RawImage>() : result;
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
        
        
        
        private void OnOrientationChanged(Orientation orientation) => rawImages.ForEach(t => t.rectTransform.localEulerAngles = t.rectTransform.localEulerAngles.SetZ(orientation != Orientation.LandscapeRight ? 0 : 180));
        
        
        
        public abstract Awaitable<bool> RequestUserCameraPermissionAsync();
        
        public async void Play()
        {
            var webcamTextureOrNull = await GetWebcamTextureOrNull();
            
            rawImages.ForEach(t => t.texture = webcamTextureOrNull);
            if (webcamTextureOrNull != null) webcamTextureOrNull.Play();
        }
        
        public void Pause()
        {
            var webcamTextureOrNull = GetWebcamTextureOrNull(false);
                        
            if (webcamTextureOrNull != null) webcamTextureOrNull.Pause();;
        }
        
        public void Stop()
        {
            var webcamTextureOrNull = GetWebcamTextureOrNull(false);
            
            if (webcamTextureOrNull != null) webcamTextureOrNull.Stop();
        }
        
        
        
        [Flags]
        public enum WebCamDeviceType
        {
            FrontFacing = 1 << 0,
        }
    }
}
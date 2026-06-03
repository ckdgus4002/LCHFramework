using System;
using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class ScreenCaptureUtility
    {
        public static async Awaitable CaptureScreenshotAsTextureAsync(RectTransform rectTransform, Func<Texture, Awaitable> callback, bool autoRelease = true)
        {
            var result = await CaptureScreenshotAsTextureAsync(rectTransform);
            await callback.Invoke(result);
            
            if (autoRelease) UnityEngine.Object.Destroy(result);
        }
        
        public static async Awaitable<Texture> CaptureScreenshotAsTextureAsync(RectTransform rectTransform)
        {
            await Awaitable.EndOfFrameAsync();
            
            var fullScreen = ScreenCapture.CaptureScreenshotAsTexture();
            
            // 네 모서리의 월드 좌표 → [0]좌하단 [1]좌상단 [2]우상단 [3]우하단
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // 좌하단/우상단을 스크린 좌표로 변환
            // Screen Space - Overlay 캔버스면 uiCamera는 null이어야 함
            var uiCamera = rectTransform.GetComponentInParent<Canvas>(true).worldCamera;
            var bottomLeft = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[0]);
            var topRight   = RectTransformUtility.WorldToScreenPoint(uiCamera, corners[2]);

            // 캡쳐 텍스처 해상도가 Screen 해상도와 다를 수 있어 스케일 보정
            var scaleX = (float)fullScreen.width  / Screen.width;
            var scaleY = (float)fullScreen.height / Screen.height;

            // 텍스처 범위를 벗어나지 않도록 클램프
            var x      = Mathf.Clamp(Mathf.RoundToInt(bottomLeft.x * scaleX), 0, fullScreen.width);
            var y      = Mathf.Clamp(Mathf.RoundToInt(bottomLeft.y * scaleY), 0, fullScreen.height);
            var width  = Mathf.Clamp(Mathf.RoundToInt((topRight.x - bottomLeft.x) * scaleX), 0, fullScreen.width - x);
            var height = Mathf.Clamp(Mathf.RoundToInt((topRight.y - bottomLeft.y) * scaleY), 0, fullScreen.height - y);

            var result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.SetPixels(fullScreen.GetPixels(x, y, width, height));
            result.Apply();
            return result;
        }
    }
}
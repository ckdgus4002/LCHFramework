using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Utilities
{
    public static class ScreenCaptureUtility
    {
        public static async Awaitable CaptureScreenshotAsTextureAsync(RectTransform rectTransform, Func<Texture, Awaitable> callback, bool autoRelease = true)
        {
            var result = await CaptureScreenshotAsTextureAsync(rectTransform);
            await callback.Invoke(result);

            if (autoRelease) Object.Destroy(result);
        }

        public static async Awaitable<Texture> CaptureScreenshotAsTextureAsync(RectTransform rectTransform, int supersize = 1)
        {
            await Awaitable.EndOfFrameAsync();
            
            var corners = new Vector3[4]; // [1],[2]
                                          // [0],[3]
            rectTransform.GetWorldCorners(corners);
            var rootCanvas = rectTransform.GetComponentInParent<Canvas>(true).rootCanvas;
            var camera = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera;
            
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture(supersize);
            
            var leftBottom = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
            var rightTop = RectTransformUtility.WorldToScreenPoint(camera, corners[2]);
            var left = Mathf.Clamp(Mathf.RoundToInt(leftBottom.x * supersize), 0, screenshot.width);
            var bottom = Mathf.Clamp(Mathf.RoundToInt(leftBottom.y * supersize), 0, screenshot.height);
            var right = Mathf.Clamp(Mathf.RoundToInt(rightTop.x * supersize), 0, screenshot.width);
            var top = Mathf.Clamp(Mathf.RoundToInt(rightTop.y * supersize), 0, screenshot.height);
            
            var width = right - left;
            var height = top - bottom;
            var result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.SetPixels(screenshot.GetPixels(left, bottom, width, height));
            result.Apply();
            Object.Destroy(screenshot);
            return result;
        }
    }
}
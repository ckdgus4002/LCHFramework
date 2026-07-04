using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Utilities
{
    public static class ScreenCaptureUtility
    {
        public static async Awaitable CaptureScreenshotAsTextureAsync(RectTransform[] rectTransforms, int supersize, Func<Texture2D[], Awaitable> callback, bool autoRelease)
        {
            var result = await CaptureScreenshotAsTextureAsync(rectTransforms, supersize);
            await callback.Invoke(result);

            if (autoRelease) for (var i = 0; i < result.Length; i++) ObjectUtility.DestroyAndSetNull(ref result[i]);
        }

        public static async Awaitable<Texture2D[]> CaptureScreenshotAsTextureAsync(RectTransform[] rectTransforms, int supersize = 1)
        {
            await Awaitable.EndOfFrameAsync();
            
            var result = new Texture2D[rectTransforms.Length];
            var screenshot = ScreenCapture.CaptureScreenshotAsTexture(supersize);
            for (var i = 0; i < rectTransforms.Length; i++)
            {
                var rectTransform = rectTransforms[i];
                var corners = new Vector3[4]; // [1],[2]
                                              // [0],[3]
                rectTransform.GetWorldCorners(corners);
                
                var rootCanvas = rectTransform.GetComponentInParent<Canvas>(true).rootCanvas;
                var camera = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera;
                var leftBottom = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
                var left = Mathf.Clamp(Mathf.RoundToInt(leftBottom.x * supersize), 0, screenshot.width);
                var bottom = Mathf.Clamp(Mathf.RoundToInt(leftBottom.y * supersize), 0, screenshot.height);
                var rightTop = RectTransformUtility.WorldToScreenPoint(camera, corners[2]);
                var width = Mathf.Clamp(Mathf.RoundToInt(rightTop.x * supersize), 0, screenshot.width) - left;
                var height = Mathf.Clamp(Mathf.RoundToInt(rightTop.y * supersize), 0, screenshot.height) - bottom;
                result[i] = new Texture2D(width, height, TextureFormat.RGBA32, false);
                result[i].SetPixels(screenshot.GetPixels(left, bottom, width, height));
                result[i].Apply();
                Object.Destroy(screenshot);
            }
            return result;
        }
    }
}
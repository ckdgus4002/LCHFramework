using System;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class CameraExtension
    {
        public static async Awaitable Capture(this Camera camera, Func<Texture2D, Awaitable> callback, bool autoRelease = true)
        {
            var result = camera.Capture();
            await callback.Invoke(result);
            
            if (autoRelease) ObjectUtility.DestroyAndSetNull(ref result);
        }
        
        public static Texture2D Capture(this Camera camera)
        {
            var w = camera.targetTexture.width;
            var h = camera.targetTexture.height;
            var prev = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;
            var result = new Texture2D(w, h, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            result.Apply();
            RenderTexture.active = prev;
            return result;
        }
    }
}
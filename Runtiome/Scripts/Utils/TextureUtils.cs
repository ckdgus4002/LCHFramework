using UnityEngine;
using UnityEngine.Rendering;

namespace LCHFramework.Utils
{
    public static class TextureUtils
    {
        public static void Copy(Texture2D source, Texture2D destination, Rect sourceRect, Vector2 destinationPosition = default)
        {
            if (SystemInfo.copyTextureSupport == CopyTextureSupport.None)
            {
                destination.SetPixels((int)destinationPosition.x, (int)destinationPosition.y, (int)sourceRect.width, (int)sourceRect.height,
                    source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height));
                destination.Apply();
            } 
            else
                Graphics.CopyTexture(source, 0, 0, (int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height, 
                    destination, 0, 0, (int)destinationPosition.x, (int)destinationPosition.y);
        }
        
        public static void FillWithTintColor(Texture2D texture2D, Color tintColor)
        {
            var pixels  = texture2D.GetPixels();
            for (var i = 0; i < pixels.Length; i++) {
                pixels[i].r *= tintColor.r;
                pixels[i].g *= tintColor.g;
                pixels[i].b *= tintColor.b;
            }
            texture2D.SetPixels(pixels);
            texture2D.Apply();
        }
    }
}
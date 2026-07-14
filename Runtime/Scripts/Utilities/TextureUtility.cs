using UnityEngine;
using UnityEngine.Rendering;

namespace LCHFramework.Utilities
{
    public static class TextureUtility
    {
        public static void Copy(Texture2D source, Texture2D destination)
        {
            if (SystemInfo.copyTextureSupport == CopyTextureSupport.None)
            {
                destination.SetPixels(source.GetPixels());
                destination.Apply();
            }
            else
                Graphics.CopyTexture(source, destination);
        }
        
        public static Texture2D Composite(Texture2D background, Texture2D overlay, Vector2Int position)
        {
            var backgroundWidth = background.width;
            var backgroundHeight = background.height;
            var backgroundPixels = background.GetPixels();
            var overlayWidth = overlay.width;
            var overlayHeight = overlay.height;
            var overlayPixels = overlay.GetPixels();
            for (var y = 0; y < overlayHeight; y++)
            {
                var destinationY = y + position.y;
                if (destinationY < 0 || backgroundHeight <= destinationY) continue;

                var backgroundRow = destinationY * backgroundWidth;
                var overlayRow = y * overlayWidth;
                for (var x = 0; x < overlayWidth; x++)
                {
                    var destinationX = x + position.x;
                    if (destinationX < 0 || backgroundWidth <= destinationX) continue;

                    var source = overlayPixels[overlayRow + x];
                    if (source.a < float.Epsilon) continue;
                    
                    var destination = backgroundPixels[backgroundRow + destinationX];
                    var outA = source.a + destination.a * (1f - source.a);
                    var weight = destination.a * (1f - source.a);
                    backgroundPixels[backgroundRow + destinationX] = new Color(
                        (source.r * source.a + destination.r * weight) / outA,
                        (source.g * source.a + destination.g * weight) / outA,
                        (source.b * source.a + destination.b * weight) / outA,
                        outA);
                }
            }

            var result = new Texture2D(backgroundWidth, backgroundHeight, TextureFormat.RGBA32, false);
            result.SetPixels(backgroundPixels);
            result.Apply();
            return result;
        }
    }
}
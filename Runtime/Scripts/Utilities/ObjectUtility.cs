using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class ObjectUtility
    {
        public static void Destroy<T>(T @object, bool allowDestroyingAssets = false) where T : Object
        {
            if (!UnityEngine.Application.isPlaying) Object.DestroyImmediate(@object, allowDestroyingAssets); 
            else Object.Destroy(@object);
        }
        
        public static void DestroyAndSetNull<T>(ref T @object) where T : Object
        {
            Object.Destroy(@object);
            @object = null;
        }
    }
}
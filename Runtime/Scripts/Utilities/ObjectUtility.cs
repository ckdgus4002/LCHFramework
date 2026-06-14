using UnityEngine;

namespace LCHFramework.Utilities
{
    public class ObjectUtility
    {
        public static void DestroyAndSetNull<T>(ref T @object) where T : Object
        {
            Destroy(@object);
            @object = null;
        }
        
        public static void Destroy<T>(T @object) where T : Object
        {
            if (!UnityEngine.Application.isPlaying) Object.DestroyImmediate(@object); 
            else Object.Destroy(@object);
        }
    }
}
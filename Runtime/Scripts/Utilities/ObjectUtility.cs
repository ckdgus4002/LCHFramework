using UnityEngine;

namespace LCHFramework.Utilities
{
    public class ObjectUtility
    {
        public static void Destroy(Object @object)
        {
            if (!UnityEngine.Application.isPlaying) Object.DestroyImmediate(@object); 
            else Object.Destroy(@object);
        }
    }
}
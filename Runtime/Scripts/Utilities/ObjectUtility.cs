using UnityEngine;

namespace LCHFramework.Utilities
{
    public class ObjectUtility
    {
        public static void DestroyAndSetNull<T>(ref T @object) where T : Object
        {
            Object.Destroy(@object);
            @object = null;
        }
    }
}
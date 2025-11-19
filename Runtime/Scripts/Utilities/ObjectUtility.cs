using UnityEngine;

namespace LCHFramework.Utilities
{
    public class ObjectUtility
    {
        public static void Destroy(GameObject texts)
        {
            if (!UnityEngine.Application.isPlaying) Object.DestroyImmediate(texts); 
            else Object.Destroy(texts);
        }
    }
}
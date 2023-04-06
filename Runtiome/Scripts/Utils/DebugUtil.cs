using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Utils
{
    public static class DebugUtil
    {
        private const string Tag = "LCHDebug";
        
        
        
        private static void LogTRS(GameObject gameObject) => Debug.Log($"[{Tag}] {gameObject.transform.Path()}, position: {gameObject.transform.position}");
    }
}
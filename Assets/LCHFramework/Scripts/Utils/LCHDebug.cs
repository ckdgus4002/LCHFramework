using UnityEngine;

namespace LCHFramework.Utils
{
    public static class LCHDebug
    {
        private const string Tag = "LCHDebug";
        
        
        
        private static void LogTRS(GameObject gameObject) => Debug.Log($"[{Tag}] {gameObject.transform.ExPath()}, position: {gameObject.transform.position}");
    }
}
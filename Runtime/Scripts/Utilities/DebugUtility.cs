using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Utilities
{
    public static class Debug
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        private static void InitializeIsDebugBuild() => isDebugBuild = UnityEngine.Debug.isDebugBuild;
        
        
        
        public static bool isDebugBuild { get; private set; }
        
        
        
        public static void Log(object message, Color color, bool onlyDebugBuild = true) => Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>", onlyDebugBuild);
        
        public static void Log(object message, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.Log(message); }
        
        public static void Log(object message, Object context, Color color, bool onlyDebugBuild = true) => Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>", context, onlyDebugBuild);
        
        public static void Log(object message, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.Log(message, context); }
        
        public static void LogFormat(string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogFormat(format, args); }
        
        public static void LogFormat(Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogFormat(context, format, args); }
        
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args); }
        
        public static void LogError(object message, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogError(message); }
        
        public static void LogError(object message, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogError(message, context); }
        
        public static void LogErrorFormat(string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogErrorFormat(format, args); }
        
        public static void LogErrorFormat(Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogErrorFormat(context, format, args); }
        
        public static void LogException(Exception exception, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogException(exception); }
        
        public static void LogException(Exception exception, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogException(exception, context); }
        
        public static void LogWarning(object message, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogWarning(message); }
        
        public static void LogWarning(object message, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogWarning(message, context); }
        
        public static void LogWarningFormat(string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogWarningFormat(format, args); }
        
        public static void LogWarningFormat(Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || isDebugBuild) UnityEngine.Debug.LogWarningFormat(context, format, args); }
    }
}
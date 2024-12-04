using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Utilities
{
    public static class Debug
    {
        public static bool isDebugBuild
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
        
        
        
        public static void Log(object message, Color color) => Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>");
        
        public static void Log(object message) { if (isDebugBuild) UnityEngine.Debug.Log(message); }
        
        public static void Log(object message, Object context, Color color) => Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>", context);
        
        public static void Log(object message, Object context) { if (isDebugBuild) UnityEngine.Debug.Log(message, context); }
        
        public static void LogFormat(string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogFormat(format, args); }
        
        public static void LogFormat(Object context, string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogFormat(context, format, args); }
        
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args); }
        
        public static void LogError(object message) { if (isDebugBuild) UnityEngine.Debug.LogError(message); }
        
        public static void LogError(object message, Object context) { if (isDebugBuild) UnityEngine.Debug.LogError(message, context); }
        
        public static void LogErrorFormat(string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogErrorFormat(format, args); }
        
        public static void LogErrorFormat(Object context, string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogErrorFormat(context, format, args); }
        
        public static void LogException(Exception exception) { if (isDebugBuild) UnityEngine.Debug.LogException(exception); }
        
        public static void LogException(Exception exception, Object context) { if (isDebugBuild) UnityEngine.Debug.LogException(exception, context); }
        
        public static void LogWarning(object message) { if (isDebugBuild) UnityEngine.Debug.LogWarning(message); }
        
        public static void LogWarning(object message, Object context) { if (isDebugBuild) UnityEngine.Debug.LogWarning(message, context); }
        
        public static void LogWarningFormat(string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogWarningFormat(format, args); }
        
        public static void LogWarningFormat(Object context, string format, object[] args) { if (isDebugBuild) UnityEngine.Debug.LogWarningFormat(context, format, args); }
    }
}
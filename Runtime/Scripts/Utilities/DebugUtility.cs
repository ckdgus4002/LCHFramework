using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Utilies
{
    public static class Debug
    {
        public static void Log(object message, Color color, bool onlyDebugBuild = true) => Log($"<color={color}>{message}</color>", onlyDebugBuild);
        
        public static void Log(object message, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.Log(message); }
        
        public static void Log(object message, Object context, Color color, bool onlyDebugBuild = true) => Log($"<color={color}>{message}</color>", context, onlyDebugBuild);
        
        public static void Log(object message, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.Log(message, context); }
        
        public static void LogFormat(string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogFormat(format, args); }
        
        public static void LogFormat(Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogFormat(context, format, args); }
        
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args); }
        
        public static void LogError(object message, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogError(message); }
        
        public static void LogError(object message, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogError(message, context); }
        
        public static void LogErrorFormat(string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogErrorFormat(format, args); }
        
        public static void LogErrorFormat(Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogErrorFormat(context, format, args); }
        
        public static void LogException(Exception exception, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogException(exception); }
        
        public static void LogException(Exception exception, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogException(exception, context); }
        
        public static void LogWarning(object message, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogWarning(message); }
        
        public static void LogWarning(object message, Object context, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogWarning(message, context); }
        
        public static void LogWarningFormat(string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogWarningFormat(format, args); }
        
        public static void LogWarningFormat(Object context, string format, /*params*/object[] args, bool onlyDebugBuild = true) { if (!onlyDebugBuild || UnityEngine.Debug.isDebugBuild) UnityEngine.Debug.LogWarningFormat(context, format, args); }
    }
}
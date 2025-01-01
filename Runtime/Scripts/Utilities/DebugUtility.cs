using System;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LCHFramework.Utilities
{
    public static class Debug
    {
        private const string ConditionString = "DEBUG";
        
        
        
        [Conditional(ConditionString)] public static void Log(object message, Color color) => Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>");
        
        [Conditional(ConditionString)] public static void Log(object message) => UnityEngine.Debug.Log(message);
        
        [Conditional(ConditionString)] public static void Log(object message, Object context, Color color) => Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{message}</color>", context);
        
        [Conditional(ConditionString)] public static void Log(object message, Object context) => UnityEngine.Debug.Log(message, context);
        
        [Conditional(ConditionString)] public static void LogFormat(string format, object[] args) => UnityEngine.Debug.LogFormat(format, args);
        
        [Conditional(ConditionString)] public static void LogFormat(Object context, string format, object[] args) => UnityEngine.Debug.LogFormat(context, format, args);
        
        [Conditional(ConditionString)] public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, object[] args) => UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
        
        [Conditional(ConditionString)] public static void LogError(object message) => UnityEngine.Debug.LogError(message);
        
        [Conditional(ConditionString)] public static void LogError(object message, Object context) => UnityEngine.Debug.LogError(message, context);
        
        [Conditional(ConditionString)] public static void LogErrorFormat(string format, object[] args) => UnityEngine.Debug.LogErrorFormat(format, args);
        
        [Conditional(ConditionString)] public static void LogErrorFormat(Object context, string format, object[] args) => UnityEngine.Debug.LogErrorFormat(context, format, args);
        
        [Conditional(ConditionString)] public static void LogException(Exception exception) => UnityEngine.Debug.LogException(exception);
        
        [Conditional(ConditionString)] public static void LogException(Exception exception, Object context) => UnityEngine.Debug.LogException(exception, context);
        
        [Conditional(ConditionString)] public static void LogWarning(object message) => UnityEngine.Debug.LogWarning(message);
        
        [Conditional(ConditionString)] public static void LogWarning(object message, Object context) => UnityEngine.Debug.LogWarning(message, context);
        
        [Conditional(ConditionString)] public static void LogWarningFormat(string format, object[] args) => UnityEngine.Debug.LogWarningFormat(format, args);
        
        [Conditional(ConditionString)] public static void LogWarningFormat(Object context, string format, object[] args) => UnityEngine.Debug.LogWarningFormat(context, format, args);
    }
}
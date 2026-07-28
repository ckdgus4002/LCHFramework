using System.Reflection;

namespace LCHFramework.Editor.Utilities
{
    public static class ConsoleWindowUtility
    {
        public static void Clear() => Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.LogEntries").GetMethod("Clear")?.Invoke(new object(), null);
    }
}
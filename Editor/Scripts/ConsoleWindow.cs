using System.Reflection;

namespace LCHFramework.Editor
{
    public static class ConsoleWindow
    {
        public static void Clear() => Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.LogEntries").GetMethod("Clear")?.Invoke(new object(), null);
    }
}
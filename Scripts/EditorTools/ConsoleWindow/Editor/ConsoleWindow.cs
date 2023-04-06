using System.Reflection;
using UnityEditor;

namespace LCHFramework.EditorTool.ConsoleWindow
{
    public static class ConsoleWindow
    {
        public static void Clear() => Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.LogEntries").GetMethod("Clear")?.Invoke(new object(), null);
    }
}
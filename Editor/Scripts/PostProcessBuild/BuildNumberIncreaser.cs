using System;
using System.Reflection;
using LCHFramework.Editor.Utilities;
using LCHFramework.Utilities.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Editor
{
    public static class BuildNumberIncreaser
    {
        [PostProcessBuild]
        private static void Increase(BuildTarget buildTarget, string pathToBuiltProject)
        {
            const string getIncrementMethodName = "GetIncrement";
            var getIncrementOrNull = AssemblyUtility.InvokeMethod($"LCHFramework.Editor.{nameof(BuildNumberIncreaser)}_{getIncrementMethodName}", getIncrementMethodName, BindingFlags.NonPublic | BindingFlags.Static, null, new object[] { buildTarget, pathToBuiltProject });
            var increment = getIncrementOrNull != null ? (int)getIncrementOrNull : 1;
            PlayerSettingsUtility.SetBuildNumber($"{Convert.ToInt32(PlayerSettingsUtility.GetBuildNumber()) + increment}");
            
            Debug.Log("BuildNumber is increased");
        }
    }
}
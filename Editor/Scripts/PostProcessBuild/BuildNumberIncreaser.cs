using System;
using System.Reflection;
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
            var getIncrementOrNull = Assembly.Load("Assembly-CSharp-Editor")?.GetType($"LCHFramework.Editor.{nameof(BuildNumberIncreaser)}_GetIncrement")?.GetMethod("GetIncrement", BindingFlags.NonPublic | BindingFlags.Static)?.Invoke(null, new object[] { buildTarget, pathToBuiltProject });
            var increment = getIncrementOrNull != null ? (int)getIncrementOrNull : 1;
            PlayerSettingsUtility.SetBuildNumber($"{Convert.ToInt32(PlayerSettingsUtility.GetBuildNumber()) + increment}");
            
            Debug.Log("BuildNumber is increased");
        }
    }
}
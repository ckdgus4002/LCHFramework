using System;
using LCHFramework.Utilities.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Editor
{
    public static class BuildNumberIncreaser
    {
        private const string MenuItemRootPath = LCHFramework.MenuItemRootPath + "/Build Number Increaser";
        private const string EnableMenuItemPath = MenuItemRootPath/* + "/" + nameof(Enabled)*/;
        
        private static readonly string EnabledPrefsKey = $"{nameof(BuildNumberIncreaser)}{nameof(Enabled)}";
        
        
        private static bool Enabled
        {
            get => EditorPrefs.GetBool(EnabledPrefsKey, true);
            set => EditorPrefs.SetBool(EnabledPrefsKey, value);
        }
        
        
        
        [MenuItem(EnableMenuItemPath)] private static void EnableMenuItem() => Menu.SetChecked(EnableMenuItemPath, Enabled = !Enabled);
        
        [PostProcessBuild]
        private static void Increase(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (!Enabled) return;
            
            PlayerSettingsUtility.SetBuildNumber($"{Convert.ToInt32(PlayerSettingsUtility.GetBuildNumber()) + 1}");
            
            Debug.Log("Build number is increased.");
        }
    }
}
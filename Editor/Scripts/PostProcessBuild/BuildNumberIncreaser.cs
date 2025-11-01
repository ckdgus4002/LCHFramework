using UnityEditor;
using UnityEditor.Callbacks;

namespace LCHFramework.Editor
{
    public static class BuildNumberIncreaser
    {
        private const string MenuItemRootPath = LCHFramework.MenuItemRootPath + "/" + "Build Number Increaser";
        private const string EnabledMenuItemPath = MenuItemRootPath/* + "/" + nameof(Enabled)*/;
        
        private static readonly string EnabledPrefsKey = $"{nameof(BuildNumberIncreaser)}{nameof(Enabled)}";
        
        
        private static bool Enabled { get => EditorPrefs.GetBool(EnabledPrefsKey, true); set => EditorPrefs.SetBool(EnabledPrefsKey, value); }
        
        
        
        [MenuItem(EnabledMenuItemPath, true)] private static bool ValidateEnabledMenuItem() { Menu.SetChecked(EnabledMenuItemPath, Enabled); return true; }
        
        [MenuItem(EnabledMenuItemPath)] private static void EnableMenuItem() { Enabled = !Enabled; EditorApplication.RepaintHierarchyWindow(); }
        
        [PostProcessBuild]
        private static void Increase(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (!Enabled) return;

            global::LCHFramework.Application.BuildNumber += 1;
            
            Debug.Log("Build number is increased.");
        }
    }
}
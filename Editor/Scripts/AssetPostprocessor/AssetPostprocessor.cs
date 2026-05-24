using UnityEditor;

namespace LCHFramework.Editor
{
    public class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        private const string MenuItemRootPath = LCHFramework.MenuItemRootPath + "/" + nameof(AssetPostprocessor);
        private const string EnabledMenuItemPath = MenuItemRootPath + "/" + nameof(Enabled);
        
        private static readonly string EnabledPrefsKey = $"{nameof(SpriteAtlasPostprocessor)}{nameof(Enabled)}";
        
        
        public static bool Enabled { get => EditorPrefs.GetBool(EnabledPrefsKey, true); set => EditorPrefs.SetBool(EnabledPrefsKey, value); }
        
        
        
        [MenuItem(EnabledMenuItemPath, true)] private static bool ValidateEnabledMenuItem() { Menu.SetChecked(EnabledMenuItemPath, Enabled); return true; }

        [MenuItem(EnabledMenuItemPath)] private static void EnabledMenuItem() { Enabled = !Enabled; }
    }
}
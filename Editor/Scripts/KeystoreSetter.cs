using System.IO;
using System.Linq;
using LCHFramework.Editor.Data;
using UnityEditor;

namespace LCHFramework.Editor
{
    public class KeystoreSetter : IKeystoreSetter
    {
        private const string MenuItemRootPath = LCHFramework.MenuItemRootPath + "/" + nameof(KeystoreSetter);
        private const string EnabledMenuItemPath = MenuItemRootPath + "/" + nameof(Enabled);
        
        private static readonly string EnabledPrefsKey = $"{nameof(KeystoreSetter)}{nameof(Enabled)}";
        
        
        private static bool Enabled
        {
            get => EditorPrefs.GetBool(EnabledPrefsKey, TypeCache.GetTypesDerivedFrom<IKeystoreSetter>().All(t => t.Assembly == Application.Assembly));
            set => EditorPrefs.SetBool(EnabledPrefsKey, value);
        }
        
        
        
        [MenuItem(EnabledMenuItemPath, true)] private static bool ValidateEnabledMenuItem() { Menu.SetChecked(EnabledMenuItemPath, Enabled); return true; }
        
        [MenuItem(EnabledMenuItemPath)] private static void EnabledMenuItem() { Enabled = !Enabled; }
        
        [MenuItem(LCHFramework.MenuItemRootPath + "/" + "Set Keystore")] private static void OnMenuItemClick() => Set(true);
        
        [InitializeOnLoadMethod] private static void InitializeOnLoadMethod() { if (Enabled) Set(); }
        
        private static void Set(bool force = false)
        {
            if (!force) return;
            
            if (string.IsNullOrWhiteSpace(PlayerSettings.Android.keystoreName)) return;
            
            var keystoreFileInfo = new FileInfo(PlayerSettings.Android.keystoreName);
            if (!keystoreFileInfo.Exists) return;
            
            var keystorePasswordFileInfo = new FileInfo($"{keystoreFileInfo.Directory!.FullName}/KeystorePassword.txt");
            if (!keystorePasswordFileInfo.Exists) return;
            
            Set(keystorePasswordFileInfo);
        }
        
        public static void Set(FileInfo keystorePasswordFileInfo)
        {
            const string KeystorePass = "KeystorePass: ";
            const string KeyaliasName = "KeyaliasName: ";
            const string KeyaliasPass = "KeyaliasPass: ";
            foreach (var line in File.ReadAllLines(keystorePasswordFileInfo.FullName))
                if (line.Contains(KeystorePass)) PlayerSettings.Android.keystorePass = line.Replace(KeystorePass, "");
                else if (line.Contains(KeyaliasName)) PlayerSettings.Android.keyaliasName = line.Replace(KeyaliasName, "");
                else if (line.Contains(KeyaliasPass)) PlayerSettings.Android.keyaliasPass = line.Replace(KeyaliasPass, "");
        }
    }
}
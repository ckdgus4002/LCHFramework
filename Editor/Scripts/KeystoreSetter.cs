using System.IO;
using UnityEditor;

namespace LCHFramework.Editor
{
    public class KeystoreSetter
    {
        private const string KeystorePass = "KeystorePass: ";
        private const string KeyaliasName = "KeyaliasName: ";
        private const string KeyaliasPass = "KeyaliasPass: ";
        
        
        
        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod() => Set();
        
        
        
        private static bool _isSet;
        private static void Set()
        {
            if (_isSet) return;

            var fileInfo = new FileInfo("../Resources/KeystorePassword.txt");
            if (!fileInfo.Exists) return;

            foreach (var line in File.ReadAllLines(fileInfo.FullName))
                if (line.Contains(KeystorePass)) PlayerSettings.Android.keystorePass = line.Replace(KeystorePass, "");
                else if (line.Contains(KeyaliasName))
                    PlayerSettings.Android.keyaliasName = line.Replace(KeyaliasName, "");
                else if (line.Contains(KeyaliasPass))
                    PlayerSettings.Android.keyaliasPass = line.Replace(KeyaliasPass, "");

            _isSet = true;
        }
    }
}
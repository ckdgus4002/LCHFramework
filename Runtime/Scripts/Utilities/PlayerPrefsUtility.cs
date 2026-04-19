using UnityEngine;

namespace LCHFramework.Utilities
{
    public class PlayerPrefsUtility
    {
        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
    }
}
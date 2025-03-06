using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LCHFramework.Extensions;
using LCHFramework.Utilities.Editor;
using UnityEditor;

namespace LCHFramework
{
    public class Application
    {
        public static string BuildNumber => isEditor ? PlayerSettingsUtility.GetBuildNumber()
            : File.Exists(LCHFramework.BuildNumberInfoFilePath) ? File.ReadAllText(LCHFramework.BuildNumberInfoFilePath)
            : string.Empty;
        
        public static bool IsSimulator
        {
            get
            {
#if UNITY_EDITOR
                return !UnityEngine.Application.isEditor;
#else
                return false;
#endif
            }
        }
        
        public static bool isEditor
        {
            get
            {
#if UNITY_2023_2_OR_NEWER
                return UnityEngine.Application.isEditor;
#elif UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }
        
#if UNITY_EDITOR
        public static IEnumerable<BuildTargetGroup> PlatformGroups
        {
            get
            {
                if (_platformGroups.IsEmpty())
                {
                    _platformGroups = new List<BuildTargetGroup>();
                    var moduleManager = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.Modules.ModuleManager");
                    var isPlatformSupportLoaded = moduleManager.GetMethod("IsPlatformSupportLoaded", BindingFlags.Static | BindingFlags.NonPublic);
                    var getTargetStringFromBuildTargetGroup = moduleManager.GetMethod("GetTargetStringFromBuildTargetGroup", BindingFlags.Static | BindingFlags.NonPublic);
                    foreach (var value in Enum.GetValues(typeof(BuildTargetGroup)))
                        if (Convert.ToBoolean(isPlatformSupportLoaded?.Invoke(null, new object[] { (string)getTargetStringFromBuildTargetGroup?.Invoke(null, new[] { value }) })))
                            _platformGroups.Add((BuildTargetGroup)value);                        
                }

                return _platformGroups;
            }
        }
        private static List<BuildTargetGroup> _platformGroups;
#endif
    }
}
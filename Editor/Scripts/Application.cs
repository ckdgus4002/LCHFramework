using System;
using System.Collections.Generic;
using System.Reflection;
using LCHFramework.Extensions;
using UnityEditor;

namespace LCHFramework.Editor
{
    public class Application
    {
        public static IEnumerable<BuildTargetGroup> PlatformGroups
        {
            get
            {
                if (_platformGroups.IsEmpty())
                {
                    _platformGroups = new List<BuildTargetGroup>();
                    var moduleManager = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.Modules.ModuleManager");
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
    }
}
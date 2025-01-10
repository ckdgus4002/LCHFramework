using System.IO;
using LCHFramework.Utilities.Editor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class BuildNumberWriter : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        
        
        public void OnPreprocessBuild(BuildReport report)
        {
            var filePath = LCHFramework.BuildNumberInfoFilePath;
            if (File.Exists(filePath)) File.Create(filePath);
            File.WriteAllText(filePath, PlayerSettingsUtility.GetBuildNumber());
            
            Debug.Log("Build number is wrote.");
        }
        
        public void OnPostprocessBuild(BuildReport report)
        {
            var filePath = LCHFramework.BuildNumberInfoFilePath;
            File.Delete(filePath);
        }
    }
}
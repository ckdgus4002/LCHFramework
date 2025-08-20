using System;
using System.Threading.Tasks;
using LCHFramework.Attributes;
using TMPro;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionAndBuildNumberText : MonoBehaviour
    {
        [Header("Version")]
        [SerializeField] private string versionFormat = "v{0}";
        
        [Header("Build Number")]
        [SerializeField] private bool showOnlyValidBuildNumber;
        [SerializeField] private string buildNumberFormat = " ({0})";
        [SerializeField] private int buildNumberLength = 10;
        
        
        private TMP_Text Text => _text == null ? GetComponent<TMP_Text>() : _text;
        private TMP_Text _text;
        
        
        
        private async void Start()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            
            SetText();
        }
        
        
        
        [Button]
        protected virtual void SetText()
        {
            var version = Application.version;
            var text = string.Format(versionFormat, version);
            
            var buildNumber = Application.BuildNumber;
            var isValidBuildNumber = -1 < buildNumber && 0 < buildNumberLength;
            if (!showOnlyValidBuildNumber || isValidBuildNumber) text += string.Format(buildNumberFormat, buildNumber);
            
            Text.text = text;
        }
    }
}
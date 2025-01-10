using System.Text;
using TMPro;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionAndBuildNumberText : LCHMonoBehaviour
    {
        [SerializeField] private bool displayVersion;
        [SerializeField] private bool displayBuildNumber;
        
        
        
        private TMP_Text Text => _text == null ? GetComponent<TMP_Text>() : _text;
        private TMP_Text _text;
        
        
        
        protected override void Start()
        {
            base.Start();

            var text = new StringBuilder();
            if (displayVersion) text.Append(UnityEngine.Application.version);

            if (!displayVersion && displayBuildNumber) text.Append(Application.BuildNumber);
            else if (displayVersion && displayBuildNumber) text.Append($" ({Application.BuildNumber})");;
            
            Text.text = $"{text}";
        }
    }
}
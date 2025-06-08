using TMPro;
using UnityEngine;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionAndBuildNumberText : LCHMonoBehaviour
    {
        [Tooltip("zero is version. one is build number.")]
        [SerializeField] private string foramt = "v{0} ({1})";
        
        
        private TMP_Text Text => _text == null ? GetComponent<TMP_Text>() : _text;
        private TMP_Text _text;
        
        
        
        protected override void Start()
        {
            base.Start();
            
            Text.text = string.Format(foramt, Application.version, Application.BuildNumber);
        }
    }
}
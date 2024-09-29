using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using LCHFramework.Utilities.Editor;
using UnityEditor;
#endif

namespace LCHFramework.Components.UI
{
    /// <summary>
    /// BuildNumber를 RunTime에서도 TMP_Text로 보여줍니다.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    public class BuildNumberText : LCHMonoBehaviour
    {
        [SerializeField] private bool displayVersion;
        
        
        
        private TMP_Text Text => _text == null ? GetComponent<TMP_Text>() : _text;
        private TMP_Text _text;
        
        
        
#if UNITY_EDITOR
        private void Update()
        {
            var text = $"{(!displayVersion ? string.Empty : Application.version)}{(!PlayerSettingsUtility.TryGetBuildNumber(out var buildNumber) ? string.Empty : $"({buildNumber})")}";
            var needDirty = Text.text != text;
            Text.text = text;
            if (needDirty) EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
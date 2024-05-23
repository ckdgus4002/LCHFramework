using LCHFramework.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.Components.UI
{
    /// <summary>
    /// VersionCode를 RunTime에서도 연동하여 (TMP)Text로 보여줍니다.
    /// </summary>
    [ExecuteAlways]
    public abstract class VersionAndVersionCode : MonoBehaviour
    {
        private void Update()
        {
            var versionAndVersionCode = Application.version + (!VersionCodeUtility.TryGetVersionCode(out var result) ? string.Empty : $"({result})");
            var needDirty = GetText() != versionAndVersionCode;
            SetText(versionAndVersionCode);
            if (needDirty) EditorUtility.SetDirty(gameObject);
        }



        protected abstract string GetText();
        
        protected abstract void SetText(string text);
    }
}
using System.Linq;
using LCHFramework.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LCHFramework.Editor
{
    public static class DefaultSceneOpener
    {
        private static EditorBuildSettingsScene DefaultSceneOrNull => _defaultSceneOrNull ??= EditorBuildSettings.scenes.FirstOrDefault();
        private static EditorBuildSettingsScene _defaultSceneOrNull;
    
        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            var noPreviousScenes = SceneManager.GetActiveScene().name == "Untitled" && EditorSceneManager.GetSceneManagerSetup().IsEmpty();
            if (noPreviousScenes) return;
            
            if (DefaultSceneOrNull == null) return;
            
            EditorApplication.delayCall += () =>
            {
                Debug.Log($"Default Scene Opened: {DefaultSceneOrNull.path}");
                OpenDefaultScene();
                InternalEditorUtility.RepaintAllViews();
            };
        }

        [MenuItem(LCHFramework.MenuItemRootPath + "/" + "Open Default Scene")]
        private static void OnMenuItemClick()
        {
            if (DefaultSceneOrNull == null) return;
            
            OpenDefaultScene();
        }

        private static void OpenDefaultScene()
        {
            EditorSceneManager.OpenScene(DefaultSceneOrNull.path);
        }
    }   
}
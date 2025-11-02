using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LCHFramework.Editor
{
    public static class DefaultSceneOpener
    {
        private static EditorBuildSettingsScene DefaultSceneOrNull => _defaultSceneOrNull ??= EditorBuildSettings.scenes.FirstOrDefault();
        private static EditorBuildSettingsScene _defaultSceneOrNull;
        
        
        
        [MenuItem("Tools/Open Default Scene", false, int.MaxValue)]
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
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LCHFramework.Editor
{
    public static class DefaultSceneOpener
    {
        private static EditorBuildSettingsScene DefaultSceneOrNull => _defaultSceneOrNull ??= EditorBuildSettings.scenes.FirstOrDefault();
        private static EditorBuildSettingsScene _defaultSceneOrNull;
        
        
        
        // [MenuItem(LCHFramework.MenuItemRootPath + "/" + "Open Default Scene")]
        [MenuItem("Tools/Open Default Scene")]
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
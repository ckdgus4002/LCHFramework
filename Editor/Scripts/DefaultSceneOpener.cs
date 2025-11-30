using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LCHFramework.Editor
{
    public static class DefaultSceneOpener
    {
        [MenuItem("Tools/Open Default Scene", false, int.MaxValue)]
        private static void OnMenuItemClick() => EditorSceneManager.OpenScene(EditorBuildSettings.scenes.First().path);
    }
}
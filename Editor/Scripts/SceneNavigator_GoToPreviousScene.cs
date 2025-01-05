using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace LCHFramework.Editor
{
    public static partial class SceneNavigator
    {
        [MenuItem(MenuItemRootPath + "/" + "Go To PreviousScene", true)]
        private static bool GoToPreviousScene()
        {
            _scenePaths = _scenePaths.Where(File.Exists).ToList();
            if (_scenePaths.Count < 2)
            {
                _currentSceneIndex = 0; 
                return false;
            }
            else if (_currentSceneIndex < 1)
            {
                _currentSceneIndex = 0;
                return false;
            }
            else
            {
                EditorSceneManager.OpenScene(_scenePaths.ElementAt(_currentSceneIndex--));
                return true;
            }
        }
    }
}
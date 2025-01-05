using System.Collections.Generic;
using System.IO;
using System.Linq;
using LCHFramework.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace LCHFramework.Editor
{
    public static class SceneNavigator
    {
        public const string MenuItemRootPath = LCHFramework.MenuItemRootPath + "/" + nameof(SceneNavigator);

        private const string GoToPreviousSceneMenuItemPath = MenuItemRootPath + "/" + "Go To Previous Scene";
        private const string GoToNextSceneMenuItemPath = MenuItemRootPath + "/" + "Go To Next Scene";
        private const string GoToFirstSceneMenuItemPath = MenuItemRootPath + "/" + "Go To First Scene";
        
        private static List<string> _scenePaths = new() { SceneManager.GetActiveScene().path };
        private static int _currentSceneIndex;
        
        
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (mode != OpenSceneMode.Single) return;
            
            _scenePaths.RemoveRange(_currentSceneIndex + 1, _scenePaths.Count - _currentSceneIndex - 1);
            _currentSceneIndex = _scenePaths.Count;
            _scenePaths.Add(scene.path);
        }
        
        [MenuItem(GoToPreviousSceneMenuItemPath, true)]
        private static bool ValidateGoToPreviousScene()
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
                return true;
        }
        
        [MenuItem(GoToPreviousSceneMenuItemPath)]
        private static void GoToPreviousScene() => EditorSceneManager.OpenScene(_scenePaths.ElementAt(_currentSceneIndex--));
        
        [MenuItem(GoToNextSceneMenuItemPath, true)]
        private static bool ValidateGoToNextScene()
        {
            _scenePaths = _scenePaths.Where(File.Exists).ToList();
            if (_scenePaths.Count < 2)
            {
                _currentSceneIndex = 0; 
                return false;
            }
            else if (_scenePaths.Count < _currentSceneIndex + 2)
            {
                _currentSceneIndex = _scenePaths.Count - 1;
                return false;
            }
            else
                return true;
        }

        [MenuItem(GoToNextSceneMenuItemPath)]
        private static void GoToNextScene() => EditorSceneManager.OpenScene(_scenePaths.ElementAt(_currentSceneIndex++));

        [MenuItem(GoToFirstSceneMenuItemPath)]
        private static void GoToFirstScene() => EditorSceneManager.OpenScene(SceneManager.GetSceneAt(0).path);
    }    
}
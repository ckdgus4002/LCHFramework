using System.Collections.Generic;
using LCHFramework.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace LCHFramework.Editor
{
    public static partial class SceneNavigator
    {
        public const string MenuItemRootPath = LCHFramework.MenuItemRootPath;
        
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
            Debug.Log($"OnSceneOpened({scene.name}, {mode})");
            
            if (mode != OpenSceneMode.Single) return;
            
            _scenePaths.RemoveRange(_currentSceneIndex + 1, _scenePaths.Count - _currentSceneIndex - 1);
            _currentSceneIndex = _scenePaths.Count;
            _scenePaths.Add(scene.path);
        }
    }    
}
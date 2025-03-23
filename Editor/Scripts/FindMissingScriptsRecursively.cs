using LCHFramework.Extensions;
using UnityEditor;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Editor
{
    public class FindMissingScriptsRecursively : EditorWindow
    {
        private static int _componentNumber;
        private static int _gameObjectNumber;
        private static int _missingScriptNumber;

        
        
        [MenuItem(LCHFramework.MenuItemRootPath + "/Find Missing Scripts Recursively")]
        public static void ShowWindow() => GetWindow(typeof(FindMissingScriptsRecursively));
        
        private static void FindInSelectedGameObjects()
        {
            _gameObjectNumber = 0;
            _componentNumber = 0;
            _missingScriptNumber = 0;
            foreach (var g in Selection.gameObjects) FindInGameObject(g);

            Debug.Log($"Searched {_gameObjectNumber} GameObjects, {_componentNumber} components, found {_missingScriptNumber} missing");
        }

        private static void FindAll()
        {
            _componentNumber = 0;
            _gameObjectNumber = 0;
            _missingScriptNumber = 0;
            
            foreach (var assetPath in AssetDatabase.GetAllAssetPaths())
            foreach (var @object in AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(SceneAsset)
                         ? new[] {AssetDatabase.LoadMainAssetAtPath(assetPath)}
                         : AssetDatabase.LoadAllAssetsAtPath(assetPath))
                if (@object != null && @object is GameObject gameObject) FindInGameObject(gameObject);
            
            Debug.Log($"Searched {_gameObjectNumber} GameObjects, {_componentNumber} components, found {_missingScriptNumber} missing");
        }


        private static void FindInGameObject(GameObject gameObject)
        {
            _gameObjectNumber++;
            var components = gameObject.GetComponents<Component>();
            for (var i = 0; i < components.Length; i++)
            {
                _componentNumber++;
                if (components[i] == null)
                {
                    _missingScriptNumber++;
                    Debug.Log($"{gameObject.transform.Path()} has an empty script attached in position: {i}", gameObject);
                }
            }

            // Now recurse through each child GO (if there are any):
            foreach (Transform childTransform in gameObject.transform) FindInGameObject(childTransform.gameObject);
        }
        
        
        
        public void OnGUI()
        {
            if (GUILayout.Button("Find Missing Scripts in selected GameObjects")) FindInSelectedGameObjects();

            if (GUILayout.Button("Find Missing Scripts")) FindAll();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Component Scanned:");
                EditorGUILayout.LabelField(_componentNumber == -1 ? "---" : $"{_componentNumber}");
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Object Scanned:");
                EditorGUILayout.LabelField(_gameObjectNumber == -1 ? "---" : $"{_gameObjectNumber}");
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Possible Missing Scripts:");
                EditorGUILayout.LabelField(_missingScriptNumber == -1 ? "---" : $"{_missingScriptNumber}");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
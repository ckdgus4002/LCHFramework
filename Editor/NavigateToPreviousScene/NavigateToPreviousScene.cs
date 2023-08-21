using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LCHFramework.Editor.NavigateToPreviousScene
{
    public static class NavigateToPreviousScene
    {
        private const string Key = "previous_scene";
        private const string SeparatorValue = "\n";
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorSceneManager.sceneOpened += SceneOpened;
        }

        private static void SceneOpened(Scene scene, OpenSceneMode mode)
        {
            try
            {
                var split = PlayerPrefs.GetString(Key, string.Empty).Split(SeparatorValue);
                var values = new List<string>(split[..Mathf.Min(split.Length, 5)]);
                if (values.Contains(scene.path)) values.Remove(scene.path);
                values.Insert(0, scene.path);
                PlayerPrefs.SetString(Key, values.Aggregate(string.Empty, (result, value) =>
                {
                    if (string.IsNullOrWhiteSpace(result)) return value;
                    else if (string.IsNullOrWhiteSpace(value)) return result;
                    else return $"{result}{SeparatorValue}{value}";
                }));
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        [MenuItem("Developer Tool/LCHFramework/Navigate To PreviousScene")]
        private static void _GoToPreviousScene() => _GoToPreviousScene(1);

        private static void _GoToPreviousScene(int index)
        {
            while (true)
            {
                var values = PlayerPrefs.GetString(Key);
                var split = values.Split(SeparatorValue);
                if (split.Length < index + 1)
                {
                    Debug.Log($"캐싱된 씬 정보가 없습니다.\nvalues:\n{values}");
                    break;
                }

                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(split[index]);
                if (sceneAsset == null)
                {
                    PlayerPrefs.SetString(Key, values.Replace(split[index], string.Empty));
                    PlayerPrefs.Save();
                    continue;
                }

                EditorSceneManager.OpenScene(split[index]);
                break;
            }
        }
    }    
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LCHFramework.Editor
{
    public static class PreviousSceneNavigator
    {
        private const string Separator = "\n";
        
        private static readonly string ScenesPrefsKey = $"{nameof(PreviousSceneNavigator)}Scenes";
        
        
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorSceneManager.sceneOpened -= SceneOpened;
            EditorSceneManager.sceneOpened += SceneOpened;
        }

        private static void SceneOpened(Scene scene, OpenSceneMode mode)
        {
            var split = PlayerPrefs.GetString(ScenesPrefsKey, string.Empty).Split(Separator);
            var values = new List<string>(split[..Mathf.Min(split.Length, 5)]);
            if (values.Contains(scene.path)) values.Remove(scene.path);
            values.Insert(0, scene.path);
            PlayerPrefs.SetString(ScenesPrefsKey, values.Aggregate(string.Empty, (result, value) => string.IsNullOrWhiteSpace(result) ? value : string.IsNullOrWhiteSpace(value) ? result : $"{result}{Separator}{value}"));
            PlayerPrefs.Save();
        }

        [MenuItem(LCHFramework.MenuItemRootPath + "/Go To PreviousScene")]
        private static void GoToPreviousScene() => GoToPreviousScene(1);

        private static void GoToPreviousScene(int index)
        {
            while (true)
            {
                var scenes = PlayerPrefs.GetString(ScenesPrefsKey);
                var split = scenes.Split(Separator);
                if (split.Length < index + 1)
                {
                    Debug.Log($"캐싱된 씬 정보가 없습니다.\n{nameof(scenes)}:\n{string.Concat(scenes)}");
                    break;
                }

                var sceneAssetOrNull = AssetDatabase.LoadAssetAtPath<SceneAsset>(split[index]);
                if (sceneAssetOrNull != null)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(split[index]);
                    break;
                }
                
                PlayerPrefs.SetString(ScenesPrefsKey, scenes.Replace(split[index], string.Empty));
                PlayerPrefs.Save();
            }
        }
    }    
}
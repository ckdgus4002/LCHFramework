using System.Reflection;
using LCHFramework.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LCHFramework.Editor
{
    public class LocalIdentifierFinder : EditorWindow
    {
        [MenuItem(LCHFramework.MenuItemRootPath + "/Local Identifier Finder")]
        private static void CreateWindow() => GetWindowWithRect(typeof(LocalIdentifierFinder), new Rect(0, 0, 800, 120));
        
        private static string GetAssetPath(string guid)
        {
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            var result = string.Empty;
            foreach (var item in rootGameObjects)
            {
                var find = FindInChildren(item.transform, int.Parse(guid));
                if (find != null)
                {
                    result = find.Path();
                    break;
                }
            }

            return string.IsNullOrWhiteSpace(result) ? "not found" : result;
        }

        private static Transform FindInChildren(Transform transform, int m_LocalIdentfierInFile)
        {
            Transform result = null;
            foreach (Transform child in transform)
            {
                var identifier = GetLocalIdentifierInFile(child.gameObject);
                if (identifier == m_LocalIdentfierInFile)
                {
                    result = child;
                    break;
                }
                var find = FindInChildren(child, m_LocalIdentfierInFile);
                if (find != null)
                {
                    result = find;
                    break;
                }
            }

            return result;
        }
        
        private static int GetLocalIdentifierInFile(Object obj) 
        {
            var inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
            var serializedObject = new SerializedObject(obj);
            inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);
            var localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");
            return localIdProp.intValue;
        }
        
        
        
        private string guid = "";
        private string path = "";
        
        
 
        private void OnGUI()
        {
            GUILayout.Label("Enter guid");
            guid = GUILayout.TextField(guid);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Get Asset Path",GUILayout.Width(120))) path = GetAssetPath(guid);
            if (GUILayout.Button("Abort", GUILayout.Width(120))) Close();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Label(path);
        }
    }
}
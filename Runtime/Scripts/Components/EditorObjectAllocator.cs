using LCHFramework.Components;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace LCHFramework
{
    public abstract class EditorObjectAllocator : LCHMonoBehaviour
    {
#if UNITY_EDITOR
        [PostProcessScene(-1)]
        public static void OnBuild()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
                foreach (var rootGameObjects in SceneManager.GetSceneAt(i).GetRootGameObjects())
                    foreach (var child in rootGameObjects.GetComponentsInChildren<EditorObjectAllocator>())
                        child.OnAllocate();
        }
#endif
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!EditorApplication.isCompiling) OnAllocate();
        }
#endif
        
        
        
        public abstract void OnAllocate();
    }
}

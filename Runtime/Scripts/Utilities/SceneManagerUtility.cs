using UnityEngine.SceneManagement;

namespace LCHFramework.Utilities
{
    public static class SceneManagerUtility
    {
        public static Scene[] GetAllScenes()
        {
            var result = new Scene[SceneManager.sceneCount];
            for (var i = 0; i < result.Length; i++)
                result[i] = SceneManager.GetSceneAt(i);

            return result;
        }
    }
}
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class GameObjectExtension
    {
        public static void SetLayerInChildren(this GameObject gameObject, string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
            foreach (Transform child in gameObject.transform)
                child.gameObject.SetLayerInChildren(layerName);
        }
    }
}
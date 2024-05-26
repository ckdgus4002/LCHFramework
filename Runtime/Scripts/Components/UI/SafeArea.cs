using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SafeArea : MonoBehaviour, IScreenSizeChanged
    {
        private static bool IsPortraitOrientation => Screen.width <= Screen.height;
        
        
        
        protected virtual float NavigationBarSize => 0; // ApplicationUtility.IsEditor ? 0 : Screen.height * .02455357142857142857142857142857f;

        private CanvasScaler CanvasScaler => _canvasScaler == null ? _canvasScaler = GetComponentInParent<CanvasScaler>() : _canvasScaler;
        private CanvasScaler _canvasScaler;
        
        private LCHMonoBehaviour LCHMonoBehaviour => _lchMonoBehaviour == null ? _lchMonoBehaviour = gameObject.GetOrAddComponent<LCHMonoBehaviour>() : _lchMonoBehaviour;
        private LCHMonoBehaviour _lchMonoBehaviour;
        
        
        
        // LCHFramework Event.
        public void OnChanged(Vector2 prev, Vector2 current)
        {
            var reverseScale = CanvasScaler.transform.localScale.x.Reverse();
            var horizontalSize = ((IsPortraitOrientation ? Screen.width : Screen.safeArea.width) - NavigationBarSize) * reverseScale;

            var verticalSize = ((IsPortraitOrientation ? Screen.safeArea.height : Screen.height) - NavigationBarSize) * reverseScale;
            LCHMonoBehaviour.RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalSize);
            LCHMonoBehaviour.RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalSize);
        }
    }
}
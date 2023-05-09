using UnityEngine;

namespace LCHFramework.Components
{
    public class ColliderSizeController : MonoBehaviour
    {
        protected Vector2 CanvasSize => new(CanvasWidth, CanvasHeight);
        
        private float CanvasWidth => Camera.main == null ? -1 : Camera.main.aspect * CanvasHeight;
        
        private float CanvasHeight => LCHFrameworkSettings.Instance.canvasSize.y;
    }
}
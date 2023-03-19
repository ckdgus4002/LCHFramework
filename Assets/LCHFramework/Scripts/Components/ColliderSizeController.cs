using LCHFramework.Modules;
using UnityEngine;

namespace LCHFramework.Components
{
    public class ColliderSizeController : LCHMonoBehaviour
    {
        protected Vector2 CanvasSize => new Vector2(CanvasWidth, CanvasHeight);
        
        private float CanvasWidth => Camera.main == null ? -1 : Camera.main.aspect * CanvasHeight;
        
        private float CanvasHeight => 1080f;
    }
}
using LCHFramework.Modules;
using UnityEngine;
using MonoBehaviour = LCHFramework.Modules.MonoBehaviour;

namespace LCHFramework.Components
{
    public class ColliderSizeController : MonoBehaviour
    {
        protected Vector2 CanvasSize => new Vector2(CanvasWidth, CanvasHeight);
        
        private float CanvasWidth => Camera.main == null ? -1 : Camera.main.aspect * CanvasHeight;
        
        private float CanvasHeight => 1080f;
    }
}
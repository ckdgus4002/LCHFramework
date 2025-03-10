using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RectDummy : Graphic, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
            => !isActiveAndEnabled || RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, sp, eventCamera);

        protected override void OnPopulateMesh(VertexHelper vh)
            => vh.Clear(); 
    }
}
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace LCHFramework.Components.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RectDummy : Graphic, ICanvasRaycastFilter
    {
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (!isActiveAndEnabled)
                return true;

            return RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, sp, eventCamera);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}
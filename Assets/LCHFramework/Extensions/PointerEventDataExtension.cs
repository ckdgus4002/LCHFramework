using UnityEngine;
using UnityEngine.EventSystems;

namespace LCHFramework.Extensions
{
    public static class PointerEventDataExtension
    {
        public static Vector3 GetMousePosition(this PointerEventData eventData, Canvas canvas, Camera camera)
            => canvas.renderMode == RenderMode.ScreenSpaceOverlay 
            ? Input.mousePosition
            : camera.ScreenToWorldPoint(eventData.position)
            ;
    }
}
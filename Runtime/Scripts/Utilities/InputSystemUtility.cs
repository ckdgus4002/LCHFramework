using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LCHFramework.Utilities
{
    public static class InputSystemUtility
    {
        public static bool TryGetWasPressedThisFramePointerOrNull(out Pointer result) => (result = GetWasPressedThisFramePointerOrNull()) != null;
        
        public static bool TryGetIsPressedPointerOrNull(out Pointer result) => (result = GetIsPressedPointerOrNull()) != null;
        
        public static bool TryGetWasReleasedThisFramePointerOrNull(out Pointer result) => (result = GetWasReleasedThisFramePointerOrNull()) != null;
        
        public static bool TryGetWasReleasedThisFramePointerPositions(out List<(Pointer, Vector2)> result) => (result = GetWasReleasedThisFramePointerPositions()).Exists();
        
        public static Pointer GetWasPressedThisFramePointerOrNull()
            => Touchscreen.current != null && Touchscreen.current.touches.Any(t => t.press.wasReleasedThisFrame) ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.wasPressedThisFrame ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame ? Mouse.current
                : null;
        
        public static Pointer GetIsPressedPointerOrNull()
            => Touchscreen.current != null && Touchscreen.current.touches.Any(t => t.press.wasReleasedThisFrame) ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.isPressed ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.isPressed ? Mouse.current
                : null;
        
        public static Pointer GetWasReleasedThisFramePointerOrNull()
            => Touchscreen.current != null && Touchscreen.current.touches.Any(t => t.press.wasReleasedThisFrame) ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.wasReleasedThisFrame ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame ? Mouse.current
                : null;
        
        public static List<(Pointer, Vector2)> GetWasReleasedThisFramePointerPositions()
        {
            var result = new List<(Pointer, Vector2)>();
            if (Touchscreen.current != null && Touchscreen.current.touches.TryWhere(t => t.press.wasReleasedThisFrame, out var touches)) result.AddRange(touches.Select(t => ((Pointer)Touchscreen.current, t.position.value)));
            if (Pen.current != null && Pen.current.tip.wasReleasedThisFrame) result.Add((Pen.current, Pen.current.position.value));
            if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame) result.Add((Mouse.current, Mouse.current.position.value));
            
            return result;
        }
    }
}
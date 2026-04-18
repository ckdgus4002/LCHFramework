using UnityEngine.InputSystem;

namespace LCHFramework.Utilities
{
    public static class InputSystemUtility
    {
        public static bool TryGetWasPressedThisFrame(out Pointer pointerOrNull)
            => (pointerOrNull = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.wasPressedThisFrame ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame ? Mouse.current
                : null) != null;
        
        public static bool TryGetIsPressed(out Pointer pointerOrNull)
            => (pointerOrNull = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.isPressed ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.isPressed ? Mouse.current
                : null) != null;
        
        public static bool TryGetWasReleasedThisFrame(out Pointer pointerOrNull)
            => (pointerOrNull = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.wasReleasedThisFrame ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame ? Mouse.current
                : null) != null;
    }
}
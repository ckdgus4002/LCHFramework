using UnityEngine.InputSystem;

namespace LCHFramework.Utilities
{
    public static class InputSystemUtility
    {
        public static bool TryGetWasPressedThisFramePointerOrNull(out Pointer result) => (result = GetWasPressedThisFramePointerOrNull()) != null;
        
        public static bool TryGetIsPressedPointerOrNull(out Pointer result) => (result = GetIsPressedPointerOrNull()) != null;
        
        public static bool TryGetWasReleasedThisFramePointerOrNull(out Pointer result) => (result = GetWasReleasedThisFramePointerOrNull()) != null;
        
        public static Pointer GetWasPressedThisFramePointerOrNull()
            => Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.wasPressedThisFrame ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame ? Mouse.current
                : null;
        
        public static Pointer GetIsPressedPointerOrNull()
            => Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.isPressed ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.isPressed ? Mouse.current
                : null;
        
        public static Pointer GetWasReleasedThisFramePointerOrNull()
            => Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame ? Touchscreen.current 
                : Pen.current != null && Pen.current.tip.wasReleasedThisFrame ? Pen.current
                : Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame ? Mouse.current
                : null;
    }
}
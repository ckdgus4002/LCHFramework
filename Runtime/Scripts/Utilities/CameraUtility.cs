using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class CameraUtility
    {
        public static bool IsAspectChanged(Camera camera, float prevAspect) => !Mathf.Approximately(camera.aspect, prevAspect);
    }
}

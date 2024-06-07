using UnityEngine;

namespace LCHFramework.Utilies
{
    public static class CameraUtility
    {
        public static bool IsAspectChanged(Camera camera, float prevAspect) => !Mathf.Approximately(camera.aspect, prevAspect);
    }
}

using UnityEngine;

namespace LCHFramework.Managers
{
    public static class QualityLevelChooser
    {
        public static QualityLevel GetQualityLevel()
        {
            var systemMemorySizeMB = UnityEngine.Device.SystemInfo.systemMemorySize;
            if (systemMemorySizeMB < 1) return QualityLevel.None;
            
            // 안드로이드만 제조사가 보급형에만 HD+ 패널을 쓰기 때문에 해상도와 성능의 상관이 높다.
            // 회전에 무관하도록 짧은 변으로 비교하고, 해상도만으로 과도하게 갈리지 않게 범위를 제한한다.
            // 그 외에는 저해상도 고성능 기기가 있고, 창 크기라 기기 성능과 무관한 플랫폼이 있다.
            var panel = UnityEngine.Device.Application.platform == RuntimePlatform.Android
                ? Mathf.Clamp(Mathf.Min(UnityEngine.Device.Screen.width, UnityEngine.Device.Screen.height) / 1080f, 0.7f, 1.5f)
                : 1.3f;
            
            return ((systemMemorySizeMB / 1024f * panel) / 1.8f) switch
            {
                >= 8 => QualityLevel.Ultra,
                >= 5 => QualityLevel.VeryHigh,
                >= 3 => QualityLevel.High,
                >= 2 => QualityLevel.Medium,
                >= 1 => QualityLevel.Low, 
                _ => QualityLevel.VeryLow,
            };
        }
        
        public enum QualityLevel
        {
            None = -1,
            VeryLow,
            Low, // 갤럭시 탭 A7.
            Medium, // 갤럭시 탭 S4.
            High,
            VeryHigh,
            Ultra,
        }
    }
}

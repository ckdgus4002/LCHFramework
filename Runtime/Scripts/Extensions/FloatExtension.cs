using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class FloatExtension
    {
        public static float Reverse(this float f)
            => 1f / f;

        public static float Negative(this float f)
            => f < 0 ? f : -f;

        /// <summary>
        /// 가장 가까운 범위값을 반환합니다. 중간이면 올림합니다.
        /// </summary>
        public static float Round(this float f, float dist)
        {
            if (f < 0)
            {
                var isRound = Mathf.Abs(f) % dist <= dist * 0.5f;
                return isRound ? f.Ceiling(dist) : f.Truncate(dist);
            }
            else
            {
                var isRound = dist * 0.5f <= f % dist;
                return isRound ? f.Ceiling(dist) : f.Truncate(dist);
            }
        }
        
        /// <summary>
        /// 이 숫자보다 큰 다음 범위값을 반환합니다.
        /// </summary>
        public static float Ceiling(this float f, float dist)
            => f < 0 ? _Truncate(f, dist) : _Ceiling(f, dist);
        
        private static float _Ceiling(float f, float dist)
            => _Truncate(f + dist, dist);

        /// <summary>
        /// 이 숫자보다 작은 범위값을 반환합니다.
        /// </summary>
        public static float Truncate(this float f, float dist)
            => f < 0 ? _Truncate(f - dist, dist) : _Truncate(f, dist);

        private static float _Truncate(float f, float dist)
            => f - (Mathf.Approximately(dist, 0) ? 0 : f % dist);

        private static bool MoreThanOrEqual(this float a, float b) => Mathf.Approximately(b, a) || b < a;

        private static bool LessThanOrEqual(this float a, float b) => a < b || Mathf.Approximately(a, b);
    }
}
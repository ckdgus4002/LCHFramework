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
        public static float ExRound(this float f, float dist)
        {
            if (f < 0)
            {
                var isRound = Mathf.Abs(f) % dist <= dist * 0.5f;
                return isRound ? f.ExCeiling(dist) : f.ExTruncate(dist);
            }
            else
            {
                var isRound = dist * 0.5f <= f % dist;
                return isRound ? f.ExCeiling(dist) : f.ExTruncate(dist);
            }
        }
        
        /// <summary>
        /// 이 숫자보다 큰 다음 범위값을 반환합니다.
        /// </summary>
        public static float ExCeiling(this float f, float dist)
            => f < 0 ? _ExTruncate(f, dist) : _ExCeiling(f, dist);
        
        private static float _ExCeiling(float f, float dist)
            => _ExTruncate(f + dist, dist);

        /// <summary>
        /// 이 숫자보다 작은 범위값을 반환합니다.
        /// </summary>
        public static float ExTruncate(this float f, float dist)
            => f < 0 ? _ExTruncate(f - dist, dist) : _ExTruncate(f, dist);

        private static float _ExTruncate(float f, float dist)
            => f - (Mathf.Approximately(dist, 0) ? 0 : f % dist);
    }
}
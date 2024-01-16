using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Matrix4x4Extensions
    {
        public static Matrix4x4 NewPosition(this Matrix4x4 matrix, Vector3 position)
            => Matrix4x4.TRS(position, matrix.rotation, matrix.lossyScale);
    }
}
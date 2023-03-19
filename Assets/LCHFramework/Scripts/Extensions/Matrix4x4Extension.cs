using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class Matrix4x4Extensions
    {
        public static Vector3 GetPosition(this Matrix4x4 matrix)
            => new Vector3(matrix.m03, matrix.m13, matrix.m23);

        public static Matrix4x4 NewPosition(this Matrix4x4 matrix, Vector3 position)
            => Matrix4x4.TRS(position, matrix.rotation, matrix.lossyScale);
    }
}
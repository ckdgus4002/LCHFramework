using UnityEngine;

namespace LCHFramework.Data
{
    public interface IScreenSizeChanged
    {
        public void OnChanged(Vector2 prev, Vector2 current);
    }
}
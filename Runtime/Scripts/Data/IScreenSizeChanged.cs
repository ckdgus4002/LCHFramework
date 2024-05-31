using UnityEngine;

namespace LCHFramework.Data
{
    public interface IScreenSizeChanged
    {
        public void OnScreenSizeChanged(Vector2 prev, Vector2 current);
    }
}
using LCHFramework.Managers;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    public class SafeArea : LCHMonoBehaviour
    {
        protected virtual void Update()
        {
            transform.localPosition = GetLocalPosition();
        }
        
        
        
        protected virtual Vector2 GetLocalPosition()
        {
            var orientationIndex = OrientationManager.Instance.GetScreenOrientationIndex();
            return orientationIndex is < 1 or > 4 ? Vector2.zero : Screen.safeArea.center - ScreenUtility.HalfSize; 
        }
    }
}
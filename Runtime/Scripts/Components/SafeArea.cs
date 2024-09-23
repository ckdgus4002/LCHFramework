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
            var orientation = OrientationManager.Instance.Orientation.Value;
            return orientation is < Orientation.Portrait or > Orientation.LandscapeRight ? Vector2.zero : Screen.safeArea.center - ScreenUtility.HalfSize; 
        }
    }
}
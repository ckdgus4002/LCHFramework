using UnityEngine;
using UnityEngine.EventSystems;

namespace LCHFramework.Components
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EventSystem))]
    public class PixelDragThresholdController : LCHMonoBehaviour
    {
        // https://developer.android.com/training/multiscreen/screendensities?hl=ko#TaskUseDP
        [SerializeField] private int mediumDensityScreenDpi = 160;
        
        
        private int _defaultPixelDragThreshold;
        
        
        private EventSystem EventSystem => _eventSystem == null ? _eventSystem = GetComponent<EventSystem>() : _eventSystem;
        private EventSystem _eventSystem;
        
        
        
        protected override void Start()
        {
            base.Start();
            
            _defaultPixelDragThreshold = EventSystem.pixelDragThreshold;
            
            SetPixelDragThresholdInch(_defaultPixelDragThreshold);
        }
        
        protected virtual void SetPixelDragThresholdInch(int defaultPixelDragThreshold)
            => EventSystem.pixelDragThreshold = Mathf.Max(defaultPixelDragThreshold,Mathf.RoundToInt(defaultPixelDragThreshold * Screen.dpi / mediumDensityScreenDpi));
    }
}

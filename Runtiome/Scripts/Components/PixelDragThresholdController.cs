using UnityEngine;
using UnityEngine.EventSystems;

namespace LCHFramework.Components
{
    [RequireComponent(typeof(EventSystem))]
    public class PixelDragThresholdController : MonoBehaviour
    {
        [SerializeField] private float pixelDragThresholdInch = 0.2f;
        
        
        private EventSystem EventSystem => _eventSystem == null ? _eventSystem = GetComponent<EventSystem>() : _eventSystem;
        private EventSystem _eventSystem;
        
        
        
        protected virtual void Start()
        {
            SetPixelDragThresholdInch(pixelDragThresholdInch);
        }
        
        protected virtual void SetPixelDragThresholdInch(float value)
        {
            EventSystem.pixelDragThreshold = Mathf.RoundToInt(Screen.dpi * value);
            pixelDragThresholdInch = value;
        }
    }
}
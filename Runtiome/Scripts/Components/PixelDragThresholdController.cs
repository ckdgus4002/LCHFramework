using UnityEngine;
using UnityEngine.EventSystems;

namespace LCHFramework.Components
{
    [RequireComponent(typeof(EventSystem))]
    public class PixelDragThresholdController : MonoBehaviour
    {
        [SerializeField] private float pixelDragThresholdInch = 0.2f;
        
        
        
        private void SetPixelDragThresholdInch(float value)
        {
            EventSystem.pixelDragThreshold = Mathf.RoundToInt(Screen.dpi * value);
            pixelDragThresholdInch = value;
        }

        private EventSystem EventSystem => _eventSystem == null ? _eventSystem = GetComponent<EventSystem>() : _eventSystem;
        private EventSystem _eventSystem;
        
        
        
        private void Start()
        {
            SetPixelDragThresholdInch(pixelDragThresholdInch);
        }
    }
}
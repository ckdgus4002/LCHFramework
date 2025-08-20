using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class TouchScreenKeyboardVisibleEventTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent onVisible;
        [SerializeField] private UnityEvent onInvisible;

        
        
        private bool? _prevVisible;
        private void Update()
        {
            if (_prevVisible != null) 
                switch ((bool)_prevVisible)
                {
                    case false when TouchScreenKeyboard.visible:
                        onVisible?.Invoke();
                        break;
                    case true when !TouchScreenKeyboard.visible:
                        onInvisible?.Invoke();
                        break;
                }

            _prevVisible = TouchScreenKeyboard.visible;
        }
    }
}
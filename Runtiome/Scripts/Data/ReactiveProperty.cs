using System;

namespace LCHFramework
{
    public class ReactiveProperty<T> where T : class
    {
        public ReactiveProperty(T value = null, Action<T, T> onCurrentValueChanged = null)
        {
            _value = value;
            OnCurrentValueChanged = onCurrentValueChanged;
        }
        
        
        
        public event Action<T, T> OnCurrentValueChanged; // prev, current.
        
        
        public T PreviousValue { get; private set; }
        
        
        public T Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                
                PreviousValue = _value;
                _value = value;
                OnCurrentValueChanged?.Invoke(PreviousValue, _value);
            }
        }
        private T _value;
    }
}

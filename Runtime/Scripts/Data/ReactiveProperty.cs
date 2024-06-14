using System;

namespace LCHFramework.Data
{
    public class ReactiveProperty<T> where T : class
    {
        public ReactiveProperty(T value = null, Action<T, T> onValueChanged = null)
        {
            _value = value;
            OnValueChanged = onValueChanged;
        }
        
        
        
        public event Action<T, T> OnValueChanged; // prevValue, value.
        
        
        public T PrevValue { get; private set; }
        
        
        public T Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                
                PrevValue = _value;
                _value = value;
                OnValueChanged?.Invoke(PrevValue, _value);
            }
        }
        private T _value;
    }
}

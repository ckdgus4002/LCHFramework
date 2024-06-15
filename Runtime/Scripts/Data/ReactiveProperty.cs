using System.Collections.Generic;

namespace LCHFramework.Data
{
    public class ReactiveProperty<T>
    {
        public ReactiveProperty(T value = default, OnValueChangedDelegate<T> onValueChanged = null)
        {
            _value = value;
            OnValueChanged = onValueChanged;
        }
        
        
        
        public event OnValueChangedDelegate<T> OnValueChanged;
        
        
        public T PrevValueOrNull { get; private set; }
        
        
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                
                PrevValueOrNull = _value;
                _value = value;
                OnValueChanged?.Invoke(PrevValueOrNull, _value);
            }
        }
        private T _value;
    }
}

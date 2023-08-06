using System;

namespace LCHFramework
{
    public class ReactiveProperty<T> where T : class
    {
        public ReactiveProperty(T currentValue)
        {
            _currentValue = currentValue;
        }
        
        
        
        public Action<T, T> onCurrentValueChanged; // prev, current.
        
        
        public T PreviousValueOrNull { get; private set; }
        
        
        public T CurrentValue
        {
            get => _currentValue;
            set
            {
                if (ReferenceEquals(_currentValue, value)) return;
                
                if (Equals(_currentValue, value)) return;
                
                PreviousValueOrNull = _currentValue;
                _currentValue = value;
                onCurrentValueChanged?.Invoke(PreviousValueOrNull, _currentValue);
            }
        }
        private T _currentValue;
    }
}

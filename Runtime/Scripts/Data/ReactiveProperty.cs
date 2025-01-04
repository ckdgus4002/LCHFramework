using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Data
{
    public class ReactiveProperty<T>
    {
        public ReactiveProperty(T value = default, T prevValue = default, OnValueChangedDelegate<T> onValueChanged = null)
        {
            _value = value;
            PrevValue = prevValue;
            OnValueChanged = onValueChanged;
        }
        
        
        
        public event OnValueChangedDelegate<T> OnValueChanged;
        
        
        public T PrevValue { get; private set; }
        
        
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                
                PrevValue = _value;
                _value = value;
                OnValueChanged?.Invoke(PrevValue, _value);
            }
        }
        private T _value;
    }
}

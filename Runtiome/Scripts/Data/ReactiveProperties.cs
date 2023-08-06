using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using Object = UnityEngine.Object;

namespace LCHFramework
{
    public class ReactiveProperties<T> where T : Object
    {
        public ReactiveProperties(T currentValue, IEnumerable<T> values, Action<T, T> onCurrentValueChanged)
        {
            _reactiveProperty = new ReactiveProperty<T>(currentValue);
            Values = values;
            OnCurrentValueChanged = onCurrentValueChanged;
        }
        
        
        
        private readonly ReactiveProperty<T> _reactiveProperty;
        
        
        public IEnumerable<T> Values { private get; set; }
        

        public Action<T, T> OnCurrentValueChanged
        {
            get => _reactiveProperty.onCurrentValueChanged;
            set => _reactiveProperty.onCurrentValueChanged = value;
        }
        

        public T PreviousValueOrNull => _reactiveProperty.PreviousValueOrNull;


        public T CurrentValue
        {
            get => _reactiveProperty.CurrentValue;
            set => _reactiveProperty.CurrentValue = value; 
        }
        
        public T FirstValue => _firstValue == null ? _firstValue = Values.First() : _firstValue;
        private T _firstValue;

        public T LeftValueOrNull
        {
            get
            {
                var currentValueIndex = Values.IndexOf(CurrentValue);
                return 1 <= currentValueIndex && currentValueIndex < Values.Count() ? Values.ElementAt(currentValueIndex - 1) : null;       
            }
        }
        
        public T RightValueOrNull
        {
            get
            {
                var currentValueIndex = Values.IndexOf(CurrentValue);
                return 0 <= currentValueIndex && currentValueIndex < Values.Count() - 1? Values.ElementAt(currentValueIndex + 1): null;
            }
        }
        
        public T LastValue => _lastValue == null ? _lastValue = Values.Last() : _lastValue;
        private T _lastValue;
    }
}

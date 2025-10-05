using System;

namespace RuntimeBindings
{
    public interface IReadOnlyUpdatableField<T>
    {
        T Value { get; }
        event Action<T> OnValueChanged;
    }
    
    public class UpdatableField<T> : IReadOnlyUpdatableField<T>
    {
        private T _value;
        private event Action<T> _onValueChanged;

        public T Value
        {
            get => _value;
            private set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    _onValueChanged?.Invoke(_value);
                }
            }
        }

        public event Action<T> OnValueChanged
        {
            add => _onValueChanged += value;
            remove => _onValueChanged -= value;
        }

        public UpdatableField()
        {
            _value = default;
        }
        
        
        public void SetValue(T newValue) => Value = newValue;
    }
}
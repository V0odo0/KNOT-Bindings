using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaintedRed
{
    [Serializable]
    public class KnotBindingsProperty<T> : IKnotBindingsPropertyT<T>
    {
        public event Action<T> Changed;

        public T Value
        {
            get => Overrides.Count == 0 ? _value : Overrides.Last().Value;
            set
            {
                var oldValue = _value;
                _value = value;
                if (Overrides.Count == 0 && !(oldValue?.Equals(value) ?? false))
                    Changed?.Invoke(_value);
            }
        }
        [SerializeField] private T _value;

        protected SortedList<int, T> Overrides => _overrides ?? (_overrides = new SortedList<int, T>());
        [NonSerialized] private SortedList<int, T> _overrides = new SortedList<int, T>();


        public KnotBindingsProperty(T value = default)
        {
            _value = value;
        }


        public void InvokeChangedIfNeeded(T oldValue)
        {
            if (!(oldValue?.Equals(Value) ?? false))
                Changed?.Invoke(Value);
        }
        
        public void AddOverride(int order, T value)
        {
            if (Overrides.ContainsKey(order))
            {
                if (!(value?.Equals(Overrides[order]) ?? false))
                {
                    var oldValue = Value;
                    Overrides[order] = value;
                    InvokeChangedIfNeeded(oldValue);
                }
            }
            else
            {
                var oldValue = Value;
                Overrides.Add(order, value);
                InvokeChangedIfNeeded(oldValue);
            }
        }

        public void RemoveOverride(int order)
        {
            if (!Overrides.ContainsKey(order))
                return;

            var oldValue = Value;
            Overrides.Remove(order);
            InvokeChangedIfNeeded(oldValue);
        }

        public void ClearOverrides()
        {
            if (Overrides.Count == 0)
                return;

            var oldValue = Value;
            Overrides.Clear();
            InvokeChangedIfNeeded(oldValue);
        }

        public bool Equals(KnotBindingsProperty<T> other)
        {
            return other != null && Value != null && Value.Equals(other.Value);
        }

        public override string ToString()
        {
            return $"{Value?.ToString()} [{Overrides.Count}]";
        }


        public static implicit operator T(KnotBindingsProperty<T> d) => d.Value;
    }
}

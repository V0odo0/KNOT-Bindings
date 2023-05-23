using System;
using System.Collections.Generic;
using System.Linq;

namespace Knot.Bindings
{
    public class KnotBindingsProperty<T> : IKnotBindingsProperty
    {
        public event PropertyChangedDelegate Changed;
        public event Action Updated;

        private readonly SortedList<int, T> _values = new SortedList<int, T>();
        

        public void Set(T value, int setterPriority = 0, object setter = null)
        {
            var oldValue = Get();
            if (_values.ContainsKey(setterPriority))
                _values[setterPriority] = value;
            else _values.Add(setterPriority, value);

            var newValue = Get();
            if (!Nullable.Equals(oldValue, newValue))
            {
                Changed?.Invoke(oldValue, newValue, setter);
                Updated?.Invoke();
            }
        }

        public T Get(T defaultValue = default)
        {
            return _values.LastOrDefault().Value ?? defaultValue;
        }

        public void Delete(int setterPriority = 0, object setter = null)
        {
            if (!_values.ContainsKey(setterPriority))
                return;

            var oldValue = Get();
            _values.Remove(setterPriority);

            var newValue = Get();
            if (!Nullable.Equals(oldValue, newValue))
            {
                Changed?.Invoke(oldValue, newValue, setter);
                Updated?.Invoke();
            }
        }

        public void Clear(object setter = null)
        {
            if (_values.Count == 0)
                return;

            var oldValue = Get();
            _values.Clear();

            Changed?.Invoke(oldValue, default, setter);
            Updated?.Invoke();
        }

        public bool Equals(KnotBindingsProperty<T> other)
        {
            return other != null && Get() != null && Get().Equals(other.Get());
        }

        public Type GetValueType() => typeof(T);


        public static implicit operator T(KnotBindingsProperty<T> d) => d.Get();

        public delegate void PropertyChangedDelegate(T oldValue, T newValue, object setter);
    }
}

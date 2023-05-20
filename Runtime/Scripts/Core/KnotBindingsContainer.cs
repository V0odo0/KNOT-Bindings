using System;
using System.Collections.Generic;

namespace Knot.Bindings
{
    public class KnotBindingsContainer
    {
        private readonly Dictionary<Type, Dictionary<string, IKnotBindingsProperty>> _properties =
            new Dictionary<Type, Dictionary<string, IKnotBindingsProperty>>();


        KnotBindingsProperty<T> Fetch<T>(string propertyName)
        {
            if (!_properties.ContainsKey(typeof(T)))
                _properties.Add(typeof(T), new Dictionary<string, IKnotBindingsProperty>());

            if (!_properties[typeof(T)].ContainsKey(propertyName))
                _properties[typeof(T)].Add(propertyName, new KnotBindingsProperty<T>());

            return (KnotBindingsProperty<T>) _properties[typeof(T)][propertyName];
        }

        public T Get<T>(string propertyName, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(propertyName))
                return defaultValue;

            if (!_properties.ContainsKey(typeof(T)))
                return defaultValue;

            if (!_properties[typeof(T)].ContainsKey(propertyName))
                return defaultValue;

            var property = (KnotBindingsProperty<T>)_properties[typeof(T)][propertyName];

            return property.Get();
        }

        public void Set<T>(string propertyName, T value, int setterPriority = 0, object setter = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            Fetch<T>(propertyName).Set(value, setterPriority, setter);
        }

        public void Delete<T>(string propertyName, int setterPriority = 0, object setter = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            Fetch<T>(propertyName).Delete(setterPriority, setter);
        }

        public void Clear<T>(string propertyName, int setterPriority = 0, object setter = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            Fetch<T>(propertyName).Clear(setter);
        }

        public bool RegisterPropertyChanged<T>(string propertyName, KnotBindingsProperty<T>.PropertyChangedDelegate propertyChangedDelegate)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyChangedDelegate == null)
                return false;

            Fetch<T>(propertyName).Changed += propertyChangedDelegate;

            return true;
        }

        public bool UnRegisterPropertyChanged<T>(string propertyName, KnotBindingsProperty<T>.PropertyChangedDelegate propertyChangedDelegate)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyChangedDelegate == null)
                return false;

            Fetch<T>(propertyName).Changed -= propertyChangedDelegate;

            return true;
        }

        public delegate void NamedPropertyChangedDelegate(string propertyName, object oldValue, object newValue, object setter);
    }
}

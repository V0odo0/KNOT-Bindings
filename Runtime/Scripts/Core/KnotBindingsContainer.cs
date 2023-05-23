using System;
using System.Collections.Generic;

namespace Knot.Bindings
{
    public class KnotBindingsContainer
    {
        private readonly Dictionary<Type, Dictionary<string, IKnotBindingsProperty>> _properties =
            new Dictionary<Type, Dictionary<string, IKnotBindingsProperty>>();


        public KnotBindingsContainer()
        {

        }

        public KnotBindingsContainer(params (string propertyName, IKnotBindingsProperty property)[] properties)
        {
            AppProperties(properties);
        }


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

        public void AppProperties(params (string propertyName, IKnotBindingsProperty property)[] properties)
        {
            foreach (var p in properties)
            {
                if (string.IsNullOrEmpty(p.propertyName) || p.property == null)
                    continue;

                if (!_properties.ContainsKey(p.property.GetValueType()))
                    _properties.Add(p.property.GetValueType(), new Dictionary<string, IKnotBindingsProperty>());

                if (!_properties[p.property.GetValueType()].ContainsKey(p.propertyName))
                    _properties[p.property.GetValueType()].Add(p.propertyName, p.property);
            }
        }

        public bool RegisterPropertyChanged<T>(string propertyName, KnotBindingsProperty<T>.PropertyChangedDelegate propertyChangedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyChangedCallback == null)
                return false;

            Fetch<T>(propertyName).Changed += propertyChangedCallback;

            return true;
        }

        public bool UnRegisterPropertyChanged<T>(string propertyName, KnotBindingsProperty<T>.PropertyChangedDelegate propertyChangedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyChangedCallback == null)
                return false;

            Fetch<T>(propertyName).Changed -= propertyChangedCallback;

            return true;
        }

        public bool RegisterPropertyUpdated<T>(string propertyName, Action updatedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || updatedCallback == null)
                return false;

            Fetch<T>(propertyName).Updated += updatedCallback;

            return true;
        }

        public bool UnRegisterPropertyUpdated<T>(string propertyName, Action updatedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || updatedCallback == null)
                return false;

            Fetch<T>(propertyName).Updated -= updatedCallback;

            return true;
        }

        public delegate void NamedPropertyChangedDelegate(string propertyName, object oldValue, object newValue, object setter);
    }
}

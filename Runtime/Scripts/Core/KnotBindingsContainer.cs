using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Knot.Bindings
{
    public class KnotBindingsContainer
    {
        public event Action<string> AnyPropertyChanged; 

        private readonly Dictionary<Type, Dictionary<string, IKnotBindingsProperty>> _properties =
            new Dictionary<Type, Dictionary<string, IKnotBindingsProperty>>();


        public KnotBindingsContainer()
        {
            CollectProperties();
        }

        public KnotBindingsContainer(params (string propertyName, IKnotBindingsProperty property)[] properties)
        {
            CollectProperties();
            AddProperties(properties);
        }

        protected void CollectProperties()
        {
            foreach (var p in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                if (p.GetValue(this, null) is IKnotBindingsProperty knotBindingsProperty)
                    AddProperty(p.Name, knotBindingsProperty);
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

            AddProperty<T>(propertyName).Set(value, setterPriority, setter);
        }

        public void Delete<T>(string propertyName, int setterPriority = 0, object setter = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            AddProperty<T>(propertyName).Delete(setterPriority, setter);
        }

        public void Clear<T>(string propertyName, int setterPriority = 0, object setter = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            AddProperty<T>(propertyName).Clear(setter);
        }

        public void AddProperties(params (string propertyName, IKnotBindingsProperty property)[] properties)
        {
            foreach (var p in properties)
                AddProperty(p.propertyName, p.property);
        }

        public void AddProperty(string propertyName, IKnotBindingsProperty property)
        {
            if (string.IsNullOrEmpty(propertyName) || property == null)
                return;

            if (!_properties.ContainsKey(property.GetValueType()))
                _properties.Add(property.GetValueType(), new Dictionary<string, IKnotBindingsProperty>());

            if (!_properties[property.GetValueType()].ContainsKey(propertyName))
            {
                property.Updated += () =>
                {
                    if (_properties.Values.Any(p => p.Values.Contains(property)))
                        AnyPropertyChanged?.Invoke(propertyName);
                };
                _properties[property.GetValueType()].Add(propertyName, property);
            }
        }

        public KnotBindingsProperty<T> AddProperty<T>(string propertyName)
        {
            AddProperty(propertyName, new KnotBindingsProperty<T>());
            return (KnotBindingsProperty<T>)_properties[typeof(T)][propertyName];
        }

        public bool HasProperty<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return false;

            if (!_properties.ContainsKey(typeof(T)))
                return false;

            if (!_properties[typeof(T)].ContainsKey(propertyName))
                return false;

            return true;
        }

        

        public bool RegisterPropertyChanged<T>(string propertyName, KnotBindingsProperty<T>.PropertyChangedDelegate propertyChangedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyChangedCallback == null)
                return false;

            AddProperty<T>(propertyName).Changed += propertyChangedCallback;

            return true;
        }

        public bool UnRegisterPropertyChanged<T>(string propertyName, KnotBindingsProperty<T>.PropertyChangedDelegate propertyChangedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || propertyChangedCallback == null)
                return false;

            AddProperty<T>(propertyName).Changed -= propertyChangedCallback;

            return true;
        }

        public bool RegisterPropertyUpdated<T>(string propertyName, Action updatedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || updatedCallback == null)
                return false;

            AddProperty<T>(propertyName).Updated += updatedCallback;

            return true;
        }

        public bool UnRegisterPropertyUpdated<T>(string propertyName, Action updatedCallback)
        {
            if (string.IsNullOrEmpty(propertyName) || updatedCallback == null)
                return false;

            AddProperty<T>(propertyName).Updated -= updatedCallback;

            return true;
        }


        public delegate void NamedPropertyChangedDelegate(string propertyName, object oldValue, object newValue, object setter);
    }
}

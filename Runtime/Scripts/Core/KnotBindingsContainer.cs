using System;
using System.Collections;
using System.Collections.Generic;

namespace PaintedRed
{
    public class KnotBindingsContainer : IReadOnlyDictionary<string, IKnotBindingsProperty>
    {
        private readonly Dictionary<string, IKnotBindingsProperty> _properties = new Dictionary<string, IKnotBindingsProperty>();


        public bool HasProperty<T>(string path)
        {
            return _properties.ContainsKey(path) && _properties[path] is IKnotBindingsPropertyT<T>;
        }

        public bool HasProperty(string path)
        {
            return _properties.ContainsKey(path);
        }

        public bool Add(string path, IKnotBindingsProperty property)
        {
            return false;
        }

        public bool Remove(string path)
        {
            if (!_properties.ContainsKey(path))
                return false;

            _properties.Remove(path);
            return true;
        }

        public bool TryGetProperty<T>(string path, out IKnotBindingsPropertyT<T> property)
        {
            property = null;
            if (!HasProperty<T>(path))
                return false;

            property = _properties[path] as IKnotBindingsPropertyT<T>;
            return true;
        }

        public bool TryGetProperty(string path, out IKnotBindingsProperty property)
        {
            return _properties.TryGetValue(path, out property);
        }

        public T GetPropertyValue<T>(string path, T defaultValue = default)
        {
            if (TryGetProperty<T>(path, out var p))
                return p.Value;

            return defaultValue;
        }
        
        
        public IEnumerator<KeyValuePair<string, IKnotBindingsProperty>> GetEnumerator() => _properties.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => _properties.Count;
        public bool ContainsKey(string key) => _properties.ContainsKey(key);
        public bool TryGetValue(string key, out IKnotBindingsProperty value) => _properties.TryGetValue(key, out value);
        public IKnotBindingsProperty this[string key] => _properties[key];
        public IEnumerable<string> Keys => _properties.Keys;
        public IEnumerable<IKnotBindingsProperty> Values => _properties.Values;
    }
}

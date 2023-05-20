using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaintedRed
{
    [Serializable]
    public class KnotBindingsPropertyWithPath<T> : KnotBindingsProperty<T>
    {
        [field:SerializeField]
        public string Path { get; set; }

        public bool IsValidPath => !string.IsNullOrEmpty(Path);

        
        public KnotBindingsPropertyWithPath(T value = default) : base(value) { }

        public KnotBindingsPropertyWithPath(string path, T value = default) : base(value)
        {
            Path = path;
        }
    }
}

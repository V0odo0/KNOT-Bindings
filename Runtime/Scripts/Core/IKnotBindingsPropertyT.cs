using System;

namespace PaintedRed
{
    public interface IKnotBindingsPropertyT<T> : IKnotBindingsProperty, IEquatable<KnotBindingsProperty<T>>
    {
        event Action<T> Changed;

        T Value { get; }

        void AddOverride(int order, T value);
    }
}

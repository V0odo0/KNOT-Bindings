using System;

namespace Knot.Bindings
{
    public interface IKnotBindingsProperty
    {
        event Action Updated;
        Type GetValueType();
    }
}

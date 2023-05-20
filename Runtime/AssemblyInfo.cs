using System.Runtime.CompilerServices;

#if UNITY_EDITOR
[assembly: InternalsVisibleTo("Knot.Bindings.Editor")]
[assembly: InternalsVisibleTo("Knot.Bindings.Tests")]
[assembly: InternalsVisibleTo("Knot.Bindings.Tests.Editor")]
#endif
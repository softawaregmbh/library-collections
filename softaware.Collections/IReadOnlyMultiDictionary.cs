using System.Collections.Generic;

namespace softaware.Collections
{
    /// <summary>
    /// Wraps an IReadOnlyDictionary whose value type is a collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the values, so that the value of the IReadOnlyDictionary is IEnumerable&lt;TValue&gt;.</typeparam>
    public interface IReadOnlyMultiDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, IEnumerable<TValue>>
#if !NETSTANDARD2_0
        where TKey : notnull
#endif
    {
    }
}

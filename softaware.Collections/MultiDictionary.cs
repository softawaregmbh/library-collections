using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace softaware.Collections
{
    /// <summary>
    /// Represents dictionary where each key can have multiple values.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class MultiDictionary<TKey, TValue> : 
        Dictionary<TKey, ICollection<TValue>>,
        IReadOnlyMultiDictionary<TKey, TValue>
#if !NETSTANDARD2_0
        where TKey : notnull
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDictionary{TKey, TValue}"/> class.
        /// </summary>
        public MultiDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="lookup">Initial data of the dictionary.</param>
        public MultiDictionary(ILookup<TKey, TValue> lookup)
        {
            foreach (var element in lookup)
            {
                this[element.Key] = element.ToList();
            }
        }

        /// <summary>
        /// Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, IEnumerable<TValue>>.Keys
        {
            get { return this.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
        IEnumerable<IEnumerable<TValue>> IReadOnlyDictionary<TKey, IEnumerable<TValue>>.Values
        {
            get { return this.Values.OfType<IEnumerable<TValue>>(); }                
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, IEnumerable<TValue>>.this[TKey key]
        {
            get { return this[key]; }
        }

        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" /> interface contains an element that has the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(
            TKey key,
            #if !NETSTANDARD2_0
            [MaybeNullWhen(false)]
            #endif
            out IEnumerable<TValue> value)
        {
            ICollection<TValue>? collection;
            bool result = this.TryGetValue(key, out collection);
            value = collection!;
            return result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>>.GetEnumerator()
        {
            foreach (var pair in this) 
            {
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(pair.Key, pair.Value);
            }
        }
    }
}

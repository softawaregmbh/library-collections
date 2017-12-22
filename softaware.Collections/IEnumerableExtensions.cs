using System;
using System.Collections.Generic;
using System.Linq;

namespace softaware.Collections
{
    /// <summary>
    /// Provides extension methods for IEnumerable&lt;T&gt;.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Creates an IEnumerable containing a single item.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="item">The item.</param>
        public static IEnumerable<T> ToEnumerable<T>(this T item) 
        {
            yield return item;
        }

        /// <summary>
        /// Executes an action for every item in an IEnumerable.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var element in elements) 
            {
                action(element);
            }
        }

        /// <summary>
        /// Creates a MultiDictionary from an IEnumerable&lt;T&gt; according to specified key selector and elements selector functions.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="valuesSelector">The values selector.</param>
        /// <returns>A MultiDictionary.</returns>
        public static MultiDictionary<TKey, TValue> ToMultiDictionary<TElement, TKey, TValue>(
            this IEnumerable<TElement> elements, 
            Func<TElement, TKey> keySelector, 
            Func<TElement, IEnumerable<TValue>> valuesSelector) 
        {
            var dictionary = new MultiDictionary<TKey, TValue>();
            foreach (var element in elements)
            {
                var key = keySelector(element);
                var values = valuesSelector(element);

                ICollection<TValue> collection;
                if (!dictionary.TryGetValue(key, out collection))
                {
                    collection = values.ToList();
                    dictionary.Add(key, collection);
                }
                else
                {
                    collection.AddRange(values);
                }
            }

            return dictionary;
        }
    }
}

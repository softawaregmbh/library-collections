using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Asynchronous version of Select that allows specifying a task-returning selector. The method then awaits all tasks and returns an array of their results.
        /// </summary>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="elements">The elements</param>
        /// <param name="selector">The asynchronous function that is applied to all elements.</param>
        /// <returns>A task that completes when all asynchronous functions are completed. Its result is an array of their results.</returns>
        public static Task<TResult[]> SelectAsync<TElement, TResult>(this IEnumerable<TElement> elements, Func<TElement, Task<TResult>> selector)
        {
            return Task.WhenAll(elements.Select(selector));
        }

        /// <summary>
        /// Returns the smallest element in a sequence based on a transform function.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <typeparam name="TComparison">The result type of the transform function which is used for comparing.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <param name="selector">The transform function.</param>
        /// <returns>The smallest element.</returns>
        public static TElement MinBy<TElement, TComparison>(this IEnumerable<TElement> elements, Func<TElement, TComparison> selector)
        {
            return MinMaxBy(elements, selector, max: false, throwWhenEmpty: true);
        }

        /// <summary>
        /// Returns the smallest element in a sequence based on a transform function, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <typeparam name="TComparison">The result type of the transform function which is used for comparing.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <param name="selector">The transform function.</param>
        /// <returns>The smallest element.</returns>
        public static TElement MinByOrDefault<TElement, TComparison>(this IEnumerable<TElement> elements, Func<TElement, TComparison> selector)
        {
            return MinMaxBy(elements, selector, max: false, throwWhenEmpty: false);
        }

        /// <summary>
        /// Returns the largest element in a sequence based on a transform function.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <typeparam name="TComparison">The result type of the transform function which is used for comparing.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <param name="selector">The transform function.</param>
        /// <returns>The largest element.</returns>
        public static TElement MaxBy<TElement, TComparison>(this IEnumerable<TElement> elements, Func<TElement, TComparison> selector)
        {
            return MinMaxBy(elements, selector, max: true, throwWhenEmpty: true);
        }

        /// <summary>
        /// Returns the largest element in a sequence based on a transform function, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <typeparam name="TComparison">The result type of the transform function which is used for comparing.</typeparam>
        /// <param name="elements">The elements.</param>
        /// <param name="selector">The transform function.</param>
        /// <returns>The largest element.</returns>
        public static TElement MaxByOrDefault<TElement, TComparison>(this IEnumerable<TElement> elements, Func<TElement, TComparison> selector)
        {
            return MinMaxBy(elements, selector, max: true, throwWhenEmpty: false);
        }

        private static TElement MinMaxBy<TElement, TComparison>(this IEnumerable<TElement> elements, Func<TElement, TComparison> selector, bool max, bool throwWhenEmpty)
        {
            var enumerator = elements.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                if (throwWhenEmpty)
                {
                    throw new InvalidOperationException("Sequence is empty.");
                }
                else
                {
                    return default;
                }
            }

            var minMax = enumerator.Current;
            var minMaxValue = selector(minMax);
            var comparer = Comparer<TComparison>.Default;

            while (enumerator.MoveNext())
            {
                var value = selector(enumerator.Current);
                var result = comparer.Compare(value, minMaxValue);
                if ((max && result > 0) || (!max && result < 0))
                {
                    minMax = enumerator.Current;
                    minMaxValue = value;
                }
            }

            return minMax;
        }
    }
}

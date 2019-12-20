using System.Collections.Generic;

namespace softaware.Collections
{
    /// <summary>
    /// Provides extension methods for ICollection&lt;T&gt;.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds multiple items to a collection.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, params T[] items) 
        {
            collection.AddRange((IEnumerable<T>)items);
        }

        /// <summary>
        /// Adds multiple items to a collection.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {            
            if (collection is List<T> list)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Removes multiple items from a collection.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void RemoveRange<T>(this ICollection<T> collection, params T[] items)
        {
            collection.RemoveRange((IEnumerable<T>)items);
        }

        /// <summary>
        /// Removes multiple items from a collection.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is List<T> list)
            {
                list.RemoveRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    collection.Remove(item);
                }
            }
        }
    }
}
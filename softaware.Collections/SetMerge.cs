using System.Collections.Generic;

namespace softaware.Collections
{
    public static class SetMerge
    {
        /// <summary>
        /// Repeatedly merges all overlapping sets until the list only contains disjoint sets.
        /// Example: (1, 2, 3), (4, 5), (6, 7), (3, 4), (7, 8) => (1, 2, 3, 4, 5), (6, 7, 8).
        /// </summary>
        /// <typeparam name="TElement">The element type of the sets.</typeparam>
        /// <param name="sets">The list of sets to merge.</param>
        public static void MergeOverlapping<TElement>(this IList<ISet<TElement>> sets)
        {
            for (int i = 0; i < sets.Count; i++)
            {
                for (int j = i + 1; j < sets.Count; j++)
                {
                    if (sets[i].Overlaps(sets[j]))
                    {
                        sets[i].UnionWith(sets[j]);
                        sets.RemoveAt(j);
                        j = i; // restart comparing the merged set with all following sets
                    }
                }
            }
        }
    }
}

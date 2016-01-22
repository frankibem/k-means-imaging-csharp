using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the index of the largest element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The input list</param>
        public static int MaxIndex<T>(this IEnumerable<T> source)
        {
            IComparer<T> comparer = Comparer<T>.Default;
            using(var iterator = source.GetEnumerator())
            {
                if(!iterator.MoveNext())
                {
                    throw new InvalidOperationException("Empty sequence");
                }

                int maxIndex = 0;
                T maxElement = iterator.Current;
                int index = 0;
                while(iterator.MoveNext())
                {
                    index++;
                    T element = iterator.Current;
                    if(comparer.Compare(element, maxElement) > 0)
                    {
                        maxElement = element;
                        maxIndex = index;
                    }
                }
                return maxIndex;
            }
        }

        /// <summary>
        /// Returns the index of the smallest element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The input list</param>
        public static int MinIndex<T>(this IEnumerable<T> source)
        {
            IComparer<T> comparer = Comparer<T>.Default;
            using(var iterator = source.GetEnumerator())
            {
                if(!iterator.MoveNext())
                {
                    throw new InvalidOperationException("Empty sequence");
                }

                int minIndex = 0;
                T minElement = iterator.Current;
                int index = 0;
                while(iterator.MoveNext())
                {
                    index++;
                    T element = iterator.Current;
                    if(comparer.Compare(element, minElement) < 0)
                    {
                        minElement = element;
                        minIndex = index;
                    }
                }
                return minIndex;
            }
        }
    }
}
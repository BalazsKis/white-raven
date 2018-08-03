using System;
using System.Collections.Generic;

namespace WhiteRaven.Shared.Library.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the specified item, then returns the list so it can be chained
        /// </summary>
        /// <typeparam name="T">Type of the list</typeparam>
        /// <param name="list">The list to add to</param>
        /// <param name="item">The item to add</param>
        /// <returns>The list itself</returns>
        public static IList<T> Add<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Adds the specified item, then returns the new array so it can be chained
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="array">The array to add to</param>
        /// <param name="item">The item to add</param>
        /// <returns>The array with the new item added</returns>
        public static T[] Add<T>(this T[] array, T item)
        {
            var result = new T[array.Length + 1];
            Array.Copy(array, result, array.Length);

            result[result.Length - 1] = item;

            return result;
        }
    }
}
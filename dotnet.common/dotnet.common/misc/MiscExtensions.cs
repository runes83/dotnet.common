using System;
using System.Collections.Generic;
using System.Threading;

namespace dotnet.common.misc
{
    /// <summary>
    ///     Misc extensions and helper methods
    /// </summary>
    public static class MiscExtensions
    {
        /// <summary>
        ///     Enables to do a action on elements in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }


       
    }
}
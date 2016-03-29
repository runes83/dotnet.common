using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        public static byte[] Combine(this byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] Combine(this byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length,third.Length);

            return ret;
        }

        public static byte[] Combine(this byte[] first,params byte[][] arrays)
        {
            byte[] ret = new byte[first.Length + arrays.Sum(x => x.Length)];
            int offset = 0;
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            offset += first.Length;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

    }
}
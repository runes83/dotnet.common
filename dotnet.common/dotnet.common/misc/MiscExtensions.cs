using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Combines 2 byte arrays into one
        /// </summary>
        /// <param name="first">First byte arry to add</param>
        /// <param name="second">Second byte array to add</param>
        /// <returns>Combined byte array</returns>
        public static byte[] Combine(this byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
        /// <summary>
        /// Combines 3 byte arrays into one
        /// </summary>
        /// <param name="first">First byte arry to add</param>
        /// <param name="second">Second byte array to add</param>
        /// <param name="third">Third byte array to add</param>
        /// <returns>Combined byte array</returns>
        public static byte[] Combine(this byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length,third.Length);

            return ret;
        }

        /// <summary>
        /// Combines 4 or more byte arrays into one
        /// </summary>
        /// <param name="first">First byte array</param>
        /// <param name="arrays">2+ byte arrays</param>
        /// <returns>Combined byte array</returns>
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

        /// <summary>
        /// Decodes a hex string to byte array
        /// </summary>
        /// <param name="hexString">String in hex cod</param>
        /// <returns>Byte array</returns>
        public static byte[] HexToBytes(this string hexString)
        {
            byte[] b = new byte[hexString.Length / 2];
            char c;
            for (var i = 0; i < hexString.Length / 2; i++)
            {
                c = hexString[i * 2];
                b[i] = (byte)((c < 0x40 ? c - 0x30 : (c < 0x47 ? c - 0x37 : c - 0x57)) << 4);
                c = hexString[i * 2 + 1];
                b[i] += (byte)(c < 0x40 ? c - 0x30 : (c < 0x47 ? c - 0x37 : c - 0x57));
            }

            return b;
        }


    }
}
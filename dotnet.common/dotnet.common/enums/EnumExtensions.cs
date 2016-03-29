using System;

namespace dotnet.common.enums
{
    /// <summary>
    /// Enum helper methods and extensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Parse from one type of enum to another assuming that they have the same values
        /// </summary>
        /// <typeparam name="R">From</typeparam>
        /// <typeparam name="T">To</typeparam>
        /// <param name="value"></param>
        /// <para name="ignoreCase">Ignore case for the Enum default: true</para>
        /// <returns></returns>
        public static T ParseEnum<R, T>(this R value,bool ignoreCase=true)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return default(T);
            return (T) Enum.Parse(typeof (T), value.ToString(), ignoreCase);
        }

        /// <summary>
        ///     Parse from string to Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <para name="ignoreCase">Ignore case for the Enum default: true</para>
        /// <returns>Enum value parsed from string</returns>
        public static T ParseEnum<T>(this string value, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            return (T) Enum.Parse(typeof (T), value, ignoreCase);
        }
    }
}
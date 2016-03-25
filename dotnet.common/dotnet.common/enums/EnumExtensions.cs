using System;

namespace dotnet.common.enums
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Parse from one type of enum to another assuming that they have the same values
        /// </summary>
        /// <typeparam name="R">From</typeparam>
        /// <typeparam name="T">To</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<R, T>(this R value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return default(T);
            return (T)Enum.Parse(typeof(T), value.ToString(), true);
        }

        /// <summary>
        /// Parse from string to Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

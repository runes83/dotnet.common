namespace dotnet.common.numbers
{
    public static class NumberExtensions
    {
        /// <summary>
        ///     Parse string to int and returns default value if the parsing errors without throwing exceptions
        /// </summary>
        /// <param name="text">String to parse for int</param>
        /// <param name="defaultValue">Value to return if the parsing errors</param>
        /// <returns>int value parsed from the string</returns>
        public static int ParseInt(this string text, int defaultValue = 0)
        {
            var result = defaultValue;
            if (!int.TryParse(text, out result))
                return defaultValue;

            return result;
        }

        /// <summary>
        ///     Parse string to double and returns default value if the parsing errors without throwing exceptions
        /// </summary>
        /// <param name="text">String to parse for double</param>
        /// <param name="defaultValue">Value to return if the parsing errors</param>
        /// <returns>double value parsed from the string</returns>
        public static double ParseDouble(this string text, double defaultValue = 0)
        {
            var result = defaultValue;
            if (!double.TryParse(text, out result))
                return defaultValue;

            return result;
        }

        /// <summary>
        ///     Parse string to long and returns default value if the parsing errors without throwing exceptions
        /// </summary>
        /// <param name="text">String to parse for long</param>
        /// <param name="defaultValue">Value to return if the parsing errors</param>
        /// <returns>long value parsed from the string</returns>
        public static long ParseLong(this string text, long defaultValue = 0)
        {
            var result = defaultValue;
            if (!long.TryParse(text, out result))
                return defaultValue;

            return result;
        }

        /// <summary>
        ///     Parse string to short and returns default value if the parsing errors without throwing exceptions
        /// </summary>
        /// <param name="text">String to parse for short</param>
        /// <param name="defaultValue">Value to return if the parsing errors</param>
        /// <returns>short value parsed from the string</returns>
        public static short ParseShort(this string text, short defaultValue = 0)
        {
            var result = defaultValue;
            if (!short.TryParse(text, out result))
                return defaultValue;

            return result;
        }
    }
}
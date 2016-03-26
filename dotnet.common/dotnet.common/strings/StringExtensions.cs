using System;
using System.Text;

namespace dotnet.common.strings
{
    /// <summary>
    ///     Common string extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Format string using the string.format method
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(format, args);
        }

        /// <summary>
        ///     Format string using the string.format method
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(provider, format, args);
        }

        /// <summary>
        ///     Enables to add lines to stringbuilder with formating
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="text"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static StringBuilder AppendLineFormat(this StringBuilder stringBuilder, string text, params object[] args)
        {
            stringBuilder.AppendLine(string.Format(text, args));
            return stringBuilder;
        }

        /// <summary>
        ///     Truncates string to the speficied lengt, if endWithDots are set length is reduced with 3 and replaced with...
        /// </summary>
        /// <param name="source">String to truncate</param>
        /// <param name="length">Number of characters</param>
        /// <param name="endwithDots">End the truncated string with ...</param>
        /// <returns></returns>
        public static string Truncate(this string source, int length, bool endwithDots = false)
        {
            if (endwithDots)
            {
                if (source.Length > length)
                    return string.Format("{0}...", source.Truncate(length - 3, true));
                return source;
            }
            return source.Truncate(length);
        }

        /// <summary>
        ///     Converts string to Base64 encoded string using UTF-8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64String(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            ;
        }

        /// <summary>
        ///     Converts a base64 string into ordinary string using UTF-8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBase64String(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        /// <summary>
        ///     Converts a base64 string into ordinary string using UTF-8 replace characters not valid in url based on jwt spec
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64UrlEncode(this string value)
        {
            var output = value.ToBase64String();
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        ///     Converts a base64  string that is urlencoded into ordinary string using UTF-8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64UrlDecode(this string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length%4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new Exception("Illegal base64url string!");
            }
            return output.FromBase64String();
        }
    }
}
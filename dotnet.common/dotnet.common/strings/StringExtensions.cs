using System;
using System.Runtime.InteropServices;
using System.Security;
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
            if (string.IsNullOrWhiteSpace(format))
                return null;

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
            if (string.IsNullOrWhiteSpace(format))
                return null;

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
        /// <returns>Truncated string</returns>
        public static string Truncate(this string source, int length, bool endwithDots = false)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            if (source.Length > length)
            {
                return endwithDots
                    ? string.Format("{0}...", source.Substring(0, length - 3))
                    : source.Substring(0, length);
            }

            return source;
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
            return value.ToBase64String()
                .Split('=')[0] // Remove any trailing '='s
                .Replace('+', '-') // 62nd char of encoding
                .Replace('/', '_'); // 63rd char of encoding
        }

        /// <summary>
        ///     Converts a base64  string that is urlencoded into ordinary string using UTF-8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64UrlDecode(this string value)
        {
            
            value = value.Replace('-', '+') // 62nd char of encoding
                         .Replace('_', '/'); // 63rd char of encoding

            switch (value.Length%4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    value += "==";
                    break; // Two pad chars
                case 3:
                    value += "=";
                    break; // One pad char
                default:
                    throw new Exception("Illegal base64url string!");
            }
            return value.FromBase64String();
        }

        /// <summary>
        ///     Converts securestring into ordinary string
        /// </summary>
        /// <param name="secureString">Secure string object</param>
        /// <returns>string object with the value from the secure string</returns>
        public static string ToUnsecureString(this SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException("secureString");

            var ptr = Marshal.SecureStringToBSTR(secureString);
            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        /// <summary>
        ///     Creates a secure string from a string, if the string is null or empty it will return a null object
        /// </summary>
        /// <param name="secureString">String value to convert into as SecureString</param>
        /// <returns>Secure string object, rember to dispose when </returns>
        public static SecureString ToSecureString(this string secureString)
        {
            if (secureString == null || string.IsNullOrWhiteSpace(secureString))
                throw new ArgumentNullException("secureString");

            unsafe
            {
                fixed (char* chars = secureString)
                {
                    var secureStringObject = new SecureString(chars, secureString.Length);
                    secureStringObject.MakeReadOnly();
                    return secureStringObject;
                }
            }
        }
    }
}
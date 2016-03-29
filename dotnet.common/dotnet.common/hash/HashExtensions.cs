using System;
using System.Security.Cryptography;
using System.Text;
using dotnet.common.misc;

namespace dotnet.common.hash
{
    public enum ByteEncoding
    {
        HEX,
        hex,
        BASE64
    };

    /// <summary>
    ///     Hash and hmac helper class
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        ///     Generates a MD5 hash of the given string
        /// </summary>
        /// <param name="value">text to hash</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>MD5 hash on the choosen format</returns>
        public static string ToMd5(this string value, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            using (var sha = new MD5CryptoServiceProvider())
            {
                return ToShaHash(value, byteEncoding, sha.ComputeHash, encoding);
            }
        }

        /// <summary>
        ///     Generates a SHA1 hash of the given string
        /// </summary>
        /// <param name="value">text to hash</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>SHA1 hash on the choosen format</returns>
        public static string ToSha1(this string value, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                return ToShaHash(value, byteEncoding, sha.ComputeHash, encoding);
            }
        }

        /// <summary>
        ///     Generates a SHA256 hash of the given string
        /// </summary>
        /// <param name="value">text to hash</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>SHA256 hash on the choosen format</returns>
        public static string ToSha256(this string value, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            using (var sha = new SHA256CryptoServiceProvider())
            {
                return ToShaHash(value, byteEncoding, sha.ComputeHash, encoding);
            }
        }

        /// <summary>
        ///     Generates a SHA384 hash of the given string
        /// </summary>
        /// <param name="value">text to hash</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>SHA384 hash on the choosen format</returns>
        public static string ToSha384(this string value, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            using (var sha = new SHA384CryptoServiceProvider())
            {
                return ToShaHash(value, byteEncoding, sha.ComputeHash, encoding);
            }
        }

        /// <summary>
        ///     Generates a SHA512 hash of the given string
        /// </summary>
        /// <param name="value">text to hash</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>SHA512 hash on the choosen format</returns>
        public static string ToSha512(this string value, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            using (var sha = new SHA512CryptoServiceProvider())
            {
                return ToShaHash(value, byteEncoding, sha.ComputeHash, encoding);
            }
        }

        /// <summary>
        ///     Generates a MD5 hash of a byte array
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <returns>Byte array with the hash result</returns>
        public static byte[] ToMd5(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new MD5CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        /// <summary>
        ///     Generates a SHA1 hash of a byte array
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <returns>Byte array with the hash result</returns>
        public static byte[] ToSha1(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        /// <summary>
        ///     Generates a SHA256 hash of a byte array
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <returns>Byte array with the hash result</returns>
        public static byte[] ToSha256(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA256CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        /// <summary>
        ///     Generates a SHA384 hash of a byte array
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <returns>Byte array with the hash result</returns>
        public static byte[] ToSha384(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA384CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        /// <summary>
        ///     Generates a SHA512 hash of a byte array
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <returns>Byte array with the hash result</returns>
        public static byte[] ToSha512(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA512CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        /// <summary>
        ///     Generates a MD5 hash of a byte array and returns as string on the choosen format
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>MD5 hash on the choosen format</returns>
        public static string ToMd5(this byte[] value, ByteEncoding byteEncoding)
        {
            if (value == null)
                return null;
            return value.ToMd5().EncodeByteArray(byteEncoding);

        }

        /// <summary>
        ///     Generates a SHA1 hash of a byte array and returns as string on the choosen format
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>SHA1 hash on the choosen format</returns>
        public static string ToSha1(this byte[] value, ByteEncoding byteEncoding)
        {
            if (value == null)
                return null;
            return value.ToSha1().EncodeByteArray(byteEncoding);

        }

        /// <summary>
        ///     Generates a SHA256 hash of a byte array and returns as string on the choosen format
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>SHA256 hash on the choosen format</returns>
        public static string ToSha256(this byte[] value, ByteEncoding byteEncoding)
        {
            if (value == null)
                return null;
            return value.ToSha256().EncodeByteArray(byteEncoding);

        }

        /// <summary>
        ///     Generates a SHA384 hash of a byte array and returns as string on the choosen format
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>SHA384 hash on the choosen format</returns>
        public static string ToSha384(this byte[] value, ByteEncoding byteEncoding)
        {
            if (value == null)
                return null;

            return value.ToSha384().EncodeByteArray(byteEncoding);
        }

        /// <summary>
        ///     Generates a SHA512 hash of a byte array and returns as string on the choosen format
        /// </summary>
        /// <param name="value">byte array to be hashed</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>SHA512 hash on the choosen format</returns>
        public static string ToSha512(this byte[] value, ByteEncoding byteEncoding)
        {
            if (value == null)
                return null;
            return value.ToSha512().EncodeByteArray(byteEncoding);

        }

        private static string ToShaHash(this string value, ByteEncoding byteEncoding,
            Func<byte[], byte[]> ComputeHash, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            return ComputeHash(encoding.GetBytes(value)).EncodeByteArray(byteEncoding);
        }

        /// <summary>
        /// Encode byte array to string
        /// </summary>
        /// <param name="hashBytes">Byte array to encode</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>String encoded byte array</returns>
        public static string EncodeByteArray(this byte[] hashBytes, ByteEncoding byteEncoding)
        {
            if (hashBytes == null)
                return null;
            if (hashBytes.Length == 0)
                return string.Empty;

            switch (byteEncoding)
            {
                case ByteEncoding.BASE64:
                    return Convert.ToBase64String(hashBytes);
                case ByteEncoding.hex:
                    return hashBytes.BytesToHex().ToLowerInvariant();

                default:
                    return hashBytes.BytesToHex();
            }
        }

        internal static string ByteToString(this byte[] buff, string format = "{0:X2}")
        {
            var sb = new StringBuilder();
            buff.ForEach(b => sb.AppendFormat(format, b));
            return sb.ToString();
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace dotnet.common.hash
{
    public static class HmacExtensions
    {
        /// <summary>
        ///     Generate a HMAC with SHA512 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">Data to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha512(this byte[] value, string secretKey, ByteEncoding byteEncoding, Encoding encoding=null)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA512(encoding.GetBytes(secretKey)))
            {
                return sha.ComputeHash(value).EncodeByteArray(byteEncoding);
            }
        }

        /// <summary>
        ///     Generate a HMAC with SHA284 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">Data to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha384(this byte[] value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA384(encoding.GetBytes(secretKey)))
            {
                return sha.ComputeHash(value).EncodeByteArray(byteEncoding);
            }
        }

        /// <summary>
        ///     Generate a HMAC with SHA256 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">Data to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha256(this byte[] value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA256(encoding.GetBytes(secretKey)))
            {
                return sha.ComputeHash(value).EncodeByteArray(byteEncoding);
            }
        }

        /// <summary>
        ///     Generate a HMAC with SHA1 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">Data to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha1(this byte[] value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA1(encoding.GetBytes(secretKey)))
            {
                return sha.ComputeHash(value).EncodeByteArray(byteEncoding);
            }
        }

        /// <summary>
        ///     Generate a HMAC with MD5 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">Data to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacMD5(this byte[] value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACMD5(encoding.GetBytes(secretKey)))
            {
                return sha.ComputeHash(value).EncodeByteArray(byteEncoding);
            }
        }

        /// <summary>
        ///     Generate a HMAC with SHA512 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">String to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha512(this string value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha512(secretKey, byteEncoding, encoding);
        }

        /// <summary>
        ///     Generate a HMAC with SHA384 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">String to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha384(this string value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha384(secretKey, byteEncoding, encoding);
        }

        /// <summary>
        ///     Generate a HMAC with SHA256 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">String to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha256(this string value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha256(secretKey, byteEncoding, encoding);
        }

        /// <summary>
        ///     Generate a HMAC with SHA1 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">String to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacSha1(this string value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha1(secretKey, byteEncoding, encoding);
        }

        /// <summary>
        ///     Generate a HMAC with MD5 for the given bytes and returns on the given format and encoding (UTF-8 as default)
        /// </summary>
        /// <param name="value">String to calculate the HMAC over</param>
        /// <param name="secretKey">Secret to be used to as key in the hmac"</param>
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <param name="encoding">What encoding to use defauts to UTF-8</param>
        /// <returns>HMAC as string</returns>
        public static string ToHmacMD5(this string value, string secretKey, ByteEncoding byteEncoding, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacMD5(secretKey, byteEncoding, encoding);
        }
    }
}
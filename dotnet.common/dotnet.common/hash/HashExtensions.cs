using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace dotnet.common.hash
{
    public enum HashFormat { HEX_UPPERCASE, HEX_LOWERCASE, BASE64 };

    public static class HashExtensions
    {
        
        public static string ToMd5(this string value, HashFormat hashFormat, Encoding encoding = null)
        {
            using (var sha = new MD5CryptoServiceProvider())
            {
                return ToShaHash(value, hashFormat, sha.ComputeHash, encoding);
            }
        }

        public static string ToSha1(this string value, HashFormat hashFormat, Encoding encoding = null)
        {
           
            using (var sha = new SHA1CryptoServiceProvider())
            {
                return ToShaHash(value, hashFormat, sha.ComputeHash, encoding);
            }
        }

        public static string ToSha256(this string value, HashFormat hashFormat, Encoding encoding = null)
        {

            using (var sha = new SHA256CryptoServiceProvider())
            {
                return ToShaHash(value, hashFormat, sha.ComputeHash, encoding);
            }
        }

        public static string ToSha384(this string value, HashFormat hashFormat, Encoding encoding = null)
        {

            using (var sha = new SHA384CryptoServiceProvider())
            {
                return ToShaHash(value, hashFormat, sha.ComputeHash, encoding);
            }
        }

        public static string ToSha512(this string value, HashFormat hashFormat, Encoding encoding = null)
        {

            using (var sha = new SHA512CryptoServiceProvider())
            {
                return ToShaHash(value, hashFormat, sha.ComputeHash, encoding);
            }
        }


        public static byte[] ToMd5(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new MD5CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        public static byte[] ToSha1(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        public static byte[] ToSha256(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA256CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        public static byte[] ToSha384(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA384CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        public static byte[] ToSha512(this byte[] value)
        {
            if (value == null)
                return null;
            using (var sha = new SHA512CryptoServiceProvider())
            {
                return sha.ComputeHash(value);
            }
        }

        public static string ToMd5(this byte[] value, HashFormat hashFormat)
        {
            if (value == null)
                return null;
            using (var sha = new MD5CryptoServiceProvider())
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToSha1(this byte[] value, HashFormat hashFormat)
        {
            if (value == null)
                return null;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToSha256(this byte[] value, HashFormat hashFormat)
        {
            if (value == null)
                return null;
            using (var sha = new SHA256CryptoServiceProvider())
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToSha384(this byte[] value, HashFormat hashFormat)
        {
            if (value == null)
                return null;
            using (var sha = new SHA384CryptoServiceProvider())
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToSha512(this byte[] value, HashFormat hashFormat)
        {
            if (value == null)
                return null;
            using (var sha = new SHA512CryptoServiceProvider())
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }


        private static string ToShaHash(this string value, HashFormat hashFormat, 
            Func<byte[],byte[]> ComputeHash , Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;
            
            return ComputeHash(encoding.GetBytes(value)).EncodeHashBytes(hashFormat);
        }

        internal static string EncodeHashBytes(this byte[] hashBytes, HashFormat hashFormat)
        {
            switch (hashFormat)
            {
                case HashFormat.BASE64:
                    return Convert.ToBase64String(hashBytes);
                    break;
                case HashFormat.HEX_LOWERCASE:
                    return hashBytes.ByteToString("{0:x2}");

                default:
                    return hashBytes.ByteToString();
            }
            
        }

        internal static string ByteToString(this byte[] buff,string format= "{0:X2}")
        {
            StringBuilder sb = new StringBuilder();
            buff.ForEach(b => sb.AppendFormat(format, b));
            return sb.ToString();
        }

    }
}

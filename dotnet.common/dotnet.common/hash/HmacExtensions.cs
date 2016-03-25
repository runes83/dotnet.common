using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace dotnet.common.hash
{
    public static class HmacExtensions
    {
        public static string ToHmacSha512(this byte[] value,string key, HashFormat hashFormat,Encoding encoding)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA512(encoding.GetBytes(key)))
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToHmacSha384(this byte[] value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA384(encoding.GetBytes(key)))
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToHmacSha256(this byte[] value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA256(encoding.GetBytes(key)))
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToHmacSha1(this byte[] value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACSHA1(encoding.GetBytes(key)))
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToHmacMD5(this byte[] value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (value == null)
                return null;

            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var sha = new HMACMD5(encoding.GetBytes(key)))
            {
                return sha.ComputeHash(value).EncodeHashBytes(hashFormat);
            }
        }

        public static string ToHmacSha512(this string value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha512(key, hashFormat, encoding);
        }

        public static string ToHmacSha384(this string value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha384(key, hashFormat, encoding);
        }

        public static string ToHmacSha256(this string value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha256(key, hashFormat, encoding);
        }

        public static string ToHmacSha1(this string value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacSha1(key, hashFormat, encoding);
        }

        public static string ToHmacMD5(this string value, string key, HashFormat hashFormat, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return encoding.GetBytes(value).ToHmacMD5(key, hashFormat, encoding);
        }
    }
}

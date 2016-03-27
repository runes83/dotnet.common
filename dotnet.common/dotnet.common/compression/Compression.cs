using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace dotnet.common.compression
{
    /// <summary>
    ///     GZIP compression helpers
    /// </summary>
    public static class Compression
    {
        /// <summary>
        ///     Compresses the string with GZIP  (UTF-8).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressWithGZIP(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text).CompressWithGZIP());
        }

        /// <summary>
        ///     Compresses the bytes with GZIP
        /// </summary>
        /// <param name="value">The bytes to be compressed.</param>
        /// <returns>Gzipped compressed bytes</returns>
        public static byte[] CompressWithGZIP(this byte[] value)
        {
            if (value == null)
                return value;

            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(value, 0, value.Length);
                }

                memoryStream.Position = 0;

                var compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);

                var gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(value.Length), 0, gZipBuffer, 0, 4);
                return gZipBuffer;
            }
        }

        /// <summary>
        ///     Decompresses the string with GZIP (UTF-8).
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns>Decompressed string</returns>
        public static string DecompressString(this string compressedText)
        {
            if (string.IsNullOrWhiteSpace(compressedText))
                return compressedText;
            return Encoding.UTF8.GetString(Convert.FromBase64String(compressedText).DecompressWithGZIP());
        }

        /// <summary>
        ///     Decompresses the bytes with GZIP
        /// </summary>
        /// <param name="compressedBytes">The compressed bytes.</param>
        /// <returns>Decompressed bytes</returns>
        public static byte[] DecompressWithGZIP(this byte[] compressedBytes)
        {
            if (compressedBytes == null)
                return compressedBytes;
            using (var memoryStream = new MemoryStream())
            {
                var dataLength = BitConverter.ToInt32(compressedBytes, 0);
                memoryStream.Write(compressedBytes, 4, compressedBytes.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return buffer;
            }
        }
    }
}
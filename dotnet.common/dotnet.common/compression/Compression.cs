using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using dotnet.common.misc;

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
                return null;

            MemoryStream compressedStream = null;
            try
            {
                compressedStream = new MemoryStream();

                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(value, 0, value.Length);
                    zipStream.Close();

                    return compressedStream.ToArray();
                }
            }
            finally
            {
                compressedStream.DoIfNotNull(x => x.Dispose());
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
                return null;

            MemoryStream compressedStream = null;
            try
            {
                compressedStream = new MemoryStream(compressedBytes);

                GZipStream zipStream = null;
                try
                {
                    zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
                finally
                {
                    zipStream.DoIfNotNull(x => x.Dispose());
                }
            }
            finally
            {
                compressedStream.DoIfNotNull(x => x.Dispose());
            }
        }
    }
}
using System;
using System.IO;
using System.Security.Cryptography;

namespace dotnet.common.encryption
{
    internal class Encryptor
    {
        private const int BlockSize = 128;

        /// <summary>
        ///     Encrypts the given array of bytes, using the configured key. Returns an <see cref="EncryptedData" /> containing the
        ///     encrypted
        ///     bytes and the generated salt.
        /// </summary>
        internal static EncryptedData Encrypt(byte[] bytes, byte[] key)
        {
            using (var aes = new AesManaged())
            {
                aes.BlockSize = BlockSize;
                aes.GenerateIV();
                aes.Key = key;

                using (var encryptor = aes.CreateEncryptor())
                using (var destination = new MemoryStream())
                using (var cryptoStream = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return new EncryptedData(destination.ToArray(), aes.IV);
                }
            }
        }

        /// <summary>
        ///     Decrypts the given <see cref="EncryptedData" /> using the configured key.
        /// </summary>
        internal static byte[] Decrypt(EncryptedData encryptedData, byte[] key)
        {
            var iv = encryptedData.Iv;
            var bytes = encryptedData.Bytes;

            using (var rijndael = new AesManaged())
            {
                rijndael.BlockSize = BlockSize;

                rijndael.IV = iv;
                rijndael.Key = key;

                using (var decryptor = rijndael.CreateDecryptor())
                using (var destination = new MemoryStream())
                using (var cryptoStream = new CryptoStream(destination, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return destination.ToArray();
                }
            }
        }
    }

    /// <summary>
    ///     Represents a chunk of encrypted data along with the salt (a.k.a. "Initialization Vector"/"IV") that was used to
    ///     encrypt it.
    /// </summary>
    internal class EncryptedData
    {
        /// <summary>
        ///     Constructs an instance from the given bytes and iv.
        /// </summary>
        internal EncryptedData(byte[] bytes, byte[] iv)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (iv == null) throw new ArgumentNullException(nameof(iv));
            Bytes = bytes;
            Iv = iv;
        }

        /// <summary>
        ///     Gets the raw data from this encrypted data instance
        /// </summary>
        internal byte[] Bytes { get; }

        /// <summary>
        ///     Gets the salt (a.k.a. "Initialization Vector"/"IV") from this encrypted data instance
        /// </summary>
        internal byte[] Iv { get; }
    }
}
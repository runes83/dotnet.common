using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using dotnet.common.misc;

namespace dotnet.common.encryption
{
    /// <summary>
    ///     Encryption service uses AES 256 bit encryption with a string (base64 encoded) key
    ///     Remember to dispose after use
    /// </summary>
    public class EncryptionService : IEncryption
    {
        private const int IvSize = 16;
        private const int BlockSize = 128;
        private  SecureString _secret;

        /// <summary>
        ///     Key to use when encryption as a base64 encoded string 256 bit
        ///     Use the GenerateNewSecret method to generate a key.
        /// </summary>
        /// <param name="secret">256 bit base64 encoed string</param>
        public EncryptionService(string secret)
        {
            _secret = new SecureString();
            secret.ForEach(x => _secret.AppendChar(x));
        }

        /// <summary>
        ///     Key to use when encryption as a base64 encoded string 256 bit
        ///     Use the GenerateNewSecret method to generate a key.
        /// </summary>
        /// <param name="secret">256 bit base64 encoed string as a SecureString</param>
        public EncryptionService(SecureString secret)
        {
            _secret = secret;
        }

        /// <summary>
        ///     Disposes internal resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~EncryptionService()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_secret != null)
                {
                    _secret.Dispose();
                    _secret = null;
                }
            }
            // free native resources if there are any.
            
        }

        /// <summary>
        ///     Encryptes file bytes and returns encrypted bytes.
        /// </summary>
        /// <param name="fileBytes">Bytes to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        public byte[] EncryptFile(byte[] fileBytes)
        {
            var result = Encryptor.Encrypt(fileBytes, Convert.FromBase64String(SecretAsString()));

            return result.Iv.Combine(result.Bytes);
        }

        /// <summary>
        ///     Encrypt file
        /// </summary>
        /// <param name="filePath">Filepath (full path) to the file that should be encrypted</param>
        /// <param name="filePathToEncryptedFile">ilepath (full path) to where to write the encrypted file</param>
        public void EncryptFile(string filePath, string filePathToEncryptedFile)
        {
            File.WriteAllBytes(filePathToEncryptedFile, File.ReadAllBytes(filePath));
        }

        /// <summary>
        ///     Encryptes string
        /// </summary>
        /// <param name="value">String value to encrypt</param>
        /// <returns>Encrypted string</returns>
        public string EncryptString(string value)
        {
            var result = Encryptor.Encrypt(Encoding.UTF8.GetBytes(value), Convert.FromBase64String(SecretAsString()));
            return string.Format("{0}|{1}", Convert.ToBase64String(result.Bytes), Convert.ToBase64String(result.Iv));
        }

        /// <summary>
        ///     Decrypt filebytes encrypted with the EncryptFile method
        /// </summary>
        /// <param name="fileBytes">Bytes to be decrypted</param>
        /// <returns>Unencrypted bytes</returns>
        public byte[] DecryptFile(byte[] fileBytes)
        {
            var iv = new byte[IvSize];
            var dataBytes = new byte[fileBytes.Length - IvSize];

            Buffer.BlockCopy(fileBytes, 0, iv, 0, IvSize);
            Buffer.BlockCopy(fileBytes, IvSize, dataBytes, 0, fileBytes.Length-IvSize);

            return Encryptor.Decrypt(new EncryptedData(dataBytes, iv), Convert.FromBase64String(SecretAsString()));
        }

        /// <summary>
        ///     Encrypt file  encrypted with the EncryptFile method
        /// </summary>
        /// <param name="filePath">Filepath (full path) to the file that should be encrypted</param>
        /// <param name="filePathToDecryptedFile">ilepath (full path) to where to write the decrypted file</param>
        public void DecryptFile(string filePath, string filePathToDecryptedFile)
        {
            File.WriteAllBytes(filePathToDecryptedFile, File.ReadAllBytes(filePath));
        }

        /// <summary>
        ///     Decrypt string encrypted with the EncryptString method
        /// </summary>
        /// <param name="value">String to be decrypted</param>
        /// <returns>Unencrypted string</returns>
        public string DecryptString(string value)
        {
            if (!value.Contains("|"))
                throw new FormatException("value must contain the | character");

            var decryptedData = value.Split('|');

            var result = Encryptor.Decrypt(
                new EncryptedData(
                    Convert.FromBase64String(decryptedData[0]),
                    Convert.FromBase64String(decryptedData[1]))
                , Convert.FromBase64String(SecretAsString())
            );

            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        ///     Generates a 256 bit base64 encoded string to be used as encryption key.
        /// </summary>
        /// <returns>256 bit base64 encoded string</returns>
        public static string GenerateNewSecret()
        {
            using (var aesManaged = new AesManaged())
            {
                aesManaged.BlockSize = BlockSize;
                aesManaged.KeySize = 256;
                aesManaged.GenerateKey();

                return Convert.ToBase64String(aesManaged.Key);
            }
        }

        private string SecretAsString()
        {
            var bstr = Marshal.SecureStringToBSTR(_secret);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }
    }
}
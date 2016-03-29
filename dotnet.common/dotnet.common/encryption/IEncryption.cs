using System;
using dotnet.common.hash;

namespace dotnet.common.encryption
{
    public interface IEncryption:IDisposable
    {
        /// <summary>
        ///     Encrypt the bytes
        /// </summary>
        /// <param name="fileBytes">Bytes to be encrypted</param>
        /// <returns>Encrypted bytes with the IV embedded</returns>
        byte[] EncryptFile(byte[] fileBytes);

        /// <summary>
        ///     Encrypts file
        /// </summary>
        /// <param name="filePath">Filepath (fullpath) to the file that should be encrypted</param>
        /// <param name="filePathToEncryptedFile">Filepath (fullpath) to where to save the file that should be encrypted</param>
        void EncryptFile(string filePath, string filePathToEncryptedFile);

        /// <summary>
        ///     Encrypt the string
        /// </summary>
        /// <param name="value">String to be encrypted</param>
        /// <returns>Encrypted string with the IV embedded</returns>
        string EncryptString(string value, ByteEncoding byteEncoding = ByteEncoding.BASE64);

        /// <summary>
        ///     Decrypt filebytes encrypted with the encryptfile method
        /// </summary>
        /// <param name="fileBytes">Bytes to be decrypted</param>
        /// <returns>Encrypted file bytes with the IV embedded</returns>
        byte[] DecryptFile(byte[] fileBytes);

        /// <summary>
        ///     Decrypts file encrypted with the encryptfile method
        /// </summary>
        /// <param name="filePath">Filepath (fullpath) to the file that should be decrypted</param>
        /// <param name="filePathToDecryptedFile">Filepath (fullpath) to where to save the file that should be decrypted</param>
        void DecryptFile(string filePath, string filePathToDecryptedFile);

        /// <summary>
        ///     Decrypt string encrypted with the Encrypt method
        /// </summary>
        /// <param name="value">Bytes to be decrypted</param>
        /// <returns>Encrypted string with the IV embedded</returns>
        string DecryptString(string value, ByteEncoding byteEncoding = ByteEncoding.BASE64);
    }
}
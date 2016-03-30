using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using dotnet.common.hash;
using dotnet.common.misc;

namespace dotnet.common.encryption
{
    /// <summary>
    ///     Encrypt files and string using a Certificate as key. The files and string larger than the key size is encrypted
    ///     with AES256.
    ///     A AES encryption key is generated and encrypted with the certificate and then emebedded in the result along with
    ///     the IV.
    ///     To decrypt the service need access to the certificates private key
    /// </summary>
    public class CertificateEncryptionService : CertificateBaseService, IEncryption
    {
        private const int IvSize = 16;
        private const int BlockSize = 128;

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificate">The certificate to encrypt with as a X509Certificate2 object</param>
        public CertificateEncryptionService(X509Certificate2 certificate) : base(certificate)
        {
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificatePath">Path to the certificate to use</param>
        /// <param name="password">Password to the ceriticate to use</param>
        public CertificateEncryptionService(string certificatePath, string password) : base(certificatePath, password)
        {
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificateDataBytes">Certificate to use as bytes</param>
        /// <param name="password">Password to the ceriticate to use</param>
        public CertificateEncryptionService(byte[] certificateDataBytes, string password) :
            base(certificateDataBytes, password)
        {
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="thumbprint">The thumprint for the ceriticate to use</param>
        /// <param name="storeName">What certificate store to use default My store</param>
        /// <param name="storeLocation">The certificate store location to use default CurrentUser</param>
        public CertificateEncryptionService(string thumbprint, StoreName storeName = StoreName.My,
            StoreLocation storeLocation = StoreLocation.CurrentUser)
            : base(thumbprint, storeName, storeLocation)
        {
        }

        /// <summary>
        ///     Encryptes file bytes and returns encrypted bytes.
        /// </summary>
        /// <param name="fileBytes">Bytes to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        public byte[] EncryptFile(byte[] fileBytes)
        {
            var key = Convert.FromBase64String(EncryptionService.GenerateNewSecret());
            var result = Encryptor.Encrypt(fileBytes, key);

            return result.Iv.Combine(publicKey.Encrypt(key, true), result.Bytes);
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
        /// <param name="byteEncoding">What format to output the result HEX (uppercase), hex (lowercase) or Base64</param>
        /// <returns>Encrypted string encoded with given format default base64</returns>
        public string EncryptString(string value, ByteEncoding byteEncoding = ByteEncoding.BASE64)
        {
            var maxLength = (publicKey.KeySize/8) - 42;
            var bytesToEncrypt = Encoding.UTF8.GetBytes(value);
            if (bytesToEncrypt.Length < maxLength)
                return string.Format("R_{0}", Convert.ToBase64String(publicKey.Encrypt(bytesToEncrypt, true)));

            var key = EncryptionService.GenerateNewSecretAsBytes();
            var result = Encryptor.Encrypt(bytesToEncrypt, key);

            return result.Iv.Combine(publicKey.Encrypt(key, true), result.Bytes).EncodeByteArray(byteEncoding);
        }

        /// <summary>
        ///     Decrypt filebytes encrypted with the EncryptFile method
        /// </summary>
        /// <param name="fileBytes">Bytes to be decrypted</param>
        /// <returns>Unencrypted bytes</returns>
        public byte[] DecryptFile(byte[] fileBytes)
        {
            var iv = new byte[IvSize];
            var key = new byte[256];
            var dataBytes = new byte[fileBytes.Length - IvSize - 256];

            Buffer.BlockCopy(fileBytes, 0, iv, 0, IvSize);
            Buffer.BlockCopy(fileBytes, IvSize, key, 0, 256);
            Buffer.BlockCopy(fileBytes, IvSize + 256, dataBytes, 0, fileBytes.Length - IvSize - 256);

            return Encryptor.Decrypt(new EncryptedData(dataBytes, iv), privateKey.Decrypt(key, true));
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
        public string DecryptString(string value, ByteEncoding byteEncoding = ByteEncoding.BASE64)
        {
            if (value.StartsWith("R_"))
            {
                value = value.Substring(2);
                var encryptedStringBytes = byteEncoding == ByteEncoding.BASE64
                    ? Convert.FromBase64String(value)
                    : value.HexToBytes();
                return Encoding.UTF8.GetString(privateKey.Decrypt(encryptedStringBytes, true));
            }
            else
            {
                var encryptedStringBytes = byteEncoding == ByteEncoding.BASE64
                    ? Convert.FromBase64String(value)
                    : value.HexToBytes();
                var iv = new byte[IvSize];
                var key = new byte[256];
                var dataBytes = new byte[encryptedStringBytes.Length - IvSize - 256];

                Buffer.BlockCopy(encryptedStringBytes, 0, iv, 0, IvSize);
                Buffer.BlockCopy(encryptedStringBytes, IvSize, key, 0, 256);
                Buffer.BlockCopy(encryptedStringBytes, IvSize + 256, dataBytes, 0,
                    encryptedStringBytes.Length - IvSize - 256);

                return
                    Encoding.UTF8.GetString(Encryptor.Decrypt(new EncryptedData(dataBytes, iv),
                        privateKey.Decrypt(key, true)));
            }
        }
    }
}
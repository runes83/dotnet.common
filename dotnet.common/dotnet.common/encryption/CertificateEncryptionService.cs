using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using dotnet.common.hash;
using dotnet.common.misc;
using dotnet.common.strings;

namespace dotnet.common.encryption
{
    /// <summary>
    ///     Encrypt files and string using a Certificate as key. The files and string larger than the key size is encrypted
    ///     with AES256.
    ///     A AES encryption key is generated and encrypted with the certificate and then emebedded in the result along with
    ///     the IV.
    ///     To decrypt the service need access to the certificates private key
    /// </summary>
    public class CertificateEncryptionService : IEncryption
    {
        private const int IvSize = 16;
        private const int BlockSize = 128;
        private X509Certificate2 certificate;

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificate">The certificate to encrypt with as a X509Certificate2 object</param>
        public CertificateEncryptionService(X509Certificate2 certificate)
        {
            this.certificate = certificate;
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificatePath">Path to the certificate to use</param>
        /// <param name="password">Password to the ceriticate to use</param>
        public CertificateEncryptionService(string certificatePath, string password)
        {
            if (!File.Exists(certificatePath))
                throw new ArgumentNullException("certificatePath is not a valid path");

            certificate = new X509Certificate2(File.ReadAllBytes(certificatePath), password,
                X509KeyStorageFlags.MachineKeySet);
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificateDataBytes">Certificate to use as bytes</param>
        /// <param name="password">Password to the ceriticate to use</param>
        public CertificateEncryptionService(byte[] certificateDataBytes, string password)
        {
            certificate = new X509Certificate2(certificateDataBytes, password, X509KeyStorageFlags.MachineKeySet);
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="thumbprint">The thumprint for the ceriticate to use</param>
        /// <param name="storeName">What certificate store to use default My store</param>
        /// <param name="storeLocation">The certificate store location to use default CurrentUser</param>
        public CertificateEncryptionService(string thumbprint, StoreName storeName = StoreName.My,
            StoreLocation storeLocation = StoreLocation.CurrentUser)
        {
            var store = new X509Store(storeName, storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Place all certificates in an X509Certificate2Collection object.
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                var currentCerts = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint.ToUpperInvariant(),
                    false);

                if (currentCerts.Count == 0)
                    currentCerts = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint.ToLowerInvariant(),
                        false);

                if (currentCerts.Count == 0)
                    throw new ApplicationException("Cannot find certificate with thumprint: {0}".FormatWith(thumbprint));
                // Return the first certificate in the collection, has the right name and is current.
                certificate = currentCerts[0];
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary>
        ///     Disposes internal resources
        /// </summary>
        public void Dispose()
        {
            certificate = null;
        }

        /// <summary>
        ///     Encryptes file bytes and returns encrypted bytes.
        /// </summary>
        /// <param name="fileBytes">Bytes to encrypt</param>
        /// <returns>Encrypted bytes</returns>
        public byte[] EncryptFile(byte[] fileBytes)
        {
            using (var rsa = certificate.PublicKey.Key as RSACryptoServiceProvider)
            {
                var key = Convert.FromBase64String(EncryptionService.GenerateNewSecret());
                var result = Encryptor.Encrypt(fileBytes, key);

                return result.Iv.Combine(rsa.Encrypt(key, true), result.Bytes);

            }
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
        public string EncryptString(string value,ByteEncoding byteEncoding=ByteEncoding.BASE64)
        {
            using (var rsa = certificate.PublicKey.Key as RSACryptoServiceProvider)
            {
                var maxLength = (rsa.KeySize/8) - 42;
                var bytesToEncrypt = Encoding.UTF8.GetBytes(value);
                if (bytesToEncrypt.Length < maxLength)
                    return string.Format("R_{0}", Convert.ToBase64String(rsa.Encrypt(bytesToEncrypt, true)));

                var key = EncryptionService.GenerateNewSecretAsBytes();
                var result = Encryptor.Encrypt(bytesToEncrypt, key);

                return result.Iv.Combine(rsa.Encrypt(key, true), result.Bytes).EncodeByteArray(byteEncoding);

            }
        }

        /// <summary>
        ///     Decrypt filebytes encrypted with the EncryptFile method
        /// </summary>
        /// <param name="fileBytes">Bytes to be decrypted</param>
        /// <returns>Unencrypted bytes</returns>
        public byte[] DecryptFile(byte[] fileBytes)
        {
            using (var rsa = certificate.PrivateKey as RSACryptoServiceProvider)
            {
                var iv = new byte[IvSize];
                var key = new byte[256];
                var dataBytes = new byte[fileBytes.Length-IvSize-256];

                Buffer.BlockCopy(fileBytes, 0, iv, 0, IvSize);
                Buffer.BlockCopy(fileBytes, IvSize, key, 0, 256);
                Buffer.BlockCopy(fileBytes, IvSize+256, dataBytes, 0, fileBytes.Length - IvSize-256);

                return Encryptor.Decrypt(new EncryptedData(dataBytes, iv), rsa.Decrypt(key,true));
            }
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
                using (var rsa = certificate.PrivateKey as RSACryptoServiceProvider)
                {
                    value = value.Substring(2);
                    var encryptedStringBytes = byteEncoding == ByteEncoding.BASE64
                       ? Convert.FromBase64String(value)
                       : value.HexToBytes();
                    return  Encoding.UTF8.GetString(rsa.Decrypt(encryptedStringBytes , true));
                }
            }
            using (var rsa = certificate.PrivateKey as RSACryptoServiceProvider)
            {
                var encryptedStringBytes = byteEncoding == ByteEncoding.BASE64
                   ? Convert.FromBase64String(value)
                   : value.HexToBytes(); 
                var iv = new byte[IvSize];
                var key = new byte[256];
                var dataBytes = new byte[encryptedStringBytes.Length - IvSize - 256];

                Buffer.BlockCopy(encryptedStringBytes, 0, iv, 0, IvSize);
                Buffer.BlockCopy(encryptedStringBytes, IvSize, key, 0, 256);
                Buffer.BlockCopy(encryptedStringBytes, IvSize + 256, dataBytes, 0, encryptedStringBytes.Length - IvSize - 256);
                
                return Encoding.UTF8.GetString(Encryptor.Decrypt(new EncryptedData(dataBytes, iv), rsa.Decrypt(key,true)));
            }
        }
    }
}
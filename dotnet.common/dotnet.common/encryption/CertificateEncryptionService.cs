using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using dotnet.common.security;
using dotnet.common.strings;

namespace dotnet.common.encryption
{
    public class CertificateEncryptionService : IEncryption, IDisposable
    {
        private X509Certificate2 certificate;
        private const int ivSize = 16;
        private const int BlockSize = 128;

        public CertificateEncryptionService(X509Certificate2 certificate)
        {
            this.certificate = certificate;
        }

        public CertificateEncryptionService(string certificatePath, string password)
        {
            if (!File.Exists(certificatePath))
                throw new ArgumentNullException("certificatePath is not a valid path");

            certificate = new X509Certificate2(File.ReadAllBytes(certificatePath), password,
                X509KeyStorageFlags.MachineKeySet);
        }

        public CertificateEncryptionService(byte[] certificateDataBytes, string password)
        {
            certificate = new X509Certificate2(certificateDataBytes, password, X509KeyStorageFlags.MachineKeySet);
        }

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
                    throw new ApplicationException("Cannot find certificate with thumprint: {0}".FormatWith(thumbprint));
                // Return the first certificate in the collection, has the right name and is current.
                certificate = currentCerts[0];
            }
            finally
            {
                store.Close();
            }
        }

        public void Dispose()
        {
            certificate = null;
        }

        public byte[] EncryptFile(byte[] fileBytes)
        {
            using (var rsa = certificate.PublicKey.Key as RSACryptoServiceProvider)
            {
                var key = Convert.FromBase64String(EncryptionService.GenerateNewSecret());
                var result = Encryptor.Encrypt(fileBytes, key);

                return result.Iv.Concat(rsa.Encrypt(key, true)).Concat(result.Bytes).ToArray();
            }
        }

        public void EncryptFile(string filePath, string filePathToEncryptedFile)
        {
            File.WriteAllBytes(filePathToEncryptedFile, File.ReadAllBytes(filePath));
        }

        public string EncryptString(string value)
        {
            using (var rsa = certificate.PublicKey.Key as RSACryptoServiceProvider)
            {
                var maxLength = (rsa.KeySize/8) - 42;
                var bytesToEncrypt = Encoding.UTF8.GetBytes(value);
                if (bytesToEncrypt.Length > maxLength)
                    return Convert.ToBase64String(rsa.Encrypt(bytesToEncrypt, true));

                var key = Convert.FromBase64String(EncryptionService.GenerateNewSecret());
                var result = Encryptor.Encrypt(bytesToEncrypt, key);

                return string.Format("{0}|{1}|{2}"
                    , Convert.ToBase64String(result.Iv)
                    , Convert.ToBase64String(rsa.Encrypt(key, true))
                    , Convert.ToBase64String(result.Bytes));
            }
        }

        public byte[] DecryptFile(byte[] fileBytes)
        {
            using (var rsa = certificate.PrivateKey as RSACryptoServiceProvider)
            {
                var iv = fileBytes.Take(ivSize).ToArray();
                var key = rsa.Decrypt(fileBytes.Skip(ivSize).Take(256).ToArray(), true);
                var result = fileBytes.Skip(ivSize + 256).ToArray();

                return Encryptor.Decrypt(new EncryptedData(result, iv), key);
            }
        }

        public void DecryptFile(string filePath, string filePathToDecryptedFile)
        {
            File.WriteAllBytes(filePathToDecryptedFile, File.ReadAllBytes(filePath));
        }

        public string DecryptString(string value)
        {
            if (value.Contains('|'))
            {
                using (var rsa = certificate.PrivateKey as RSACryptoServiceProvider)
                {
                    return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(value), true));
                }
            }
            using (var rsa = certificate.PrivateKey as RSACryptoServiceProvider)
            {
                var encryptedContent = value.Split('|');
                var iv = Convert.FromBase64String(encryptedContent[0]);
                var key = rsa.Decrypt(Convert.FromBase64String(encryptedContent[1]), true);
                var dataBytes = Convert.FromBase64String(encryptedContent[2]);

                return Encoding.UTF8.GetString(Encryptor.Decrypt(new EncryptedData(dataBytes, iv), key));
            }
        }
    }
}
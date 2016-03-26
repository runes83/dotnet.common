using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace dotnet.common.encryption
{
    public class EncryptionService : IEncryption, IDisposable
    {
        private const int ivSize = 16;
        private const int BlockSize = 128;
        private readonly SecureString _secret;

        public EncryptionService(string secret)
        {
            _secret = new SecureString();
            secret.ForEach(x => _secret.AppendChar(x));
        }

        public void Dispose()
        {
            _secret.Dispose();
        }

        public byte[] EncryptFile(byte[] fileBytes)
        {
            var result = Encryptor.Encrypt(fileBytes, Convert.FromBase64String(SecretAsString()));

            return result.Iv.Concat(result.Bytes).ToArray();
        }

        public void EncryptFile(string filePath, string filePathToEncryptedFile)
        {
            File.WriteAllBytes(filePathToEncryptedFile, File.ReadAllBytes(filePath));
        }

        public string EncryptString(string value)
        {
            var result = Encryptor.Encrypt(Encoding.UTF8.GetBytes(value), Convert.FromBase64String(SecretAsString()));
            return string.Format("{0}|{1}", Convert.ToBase64String(result.Bytes), Convert.ToBase64String(result.Iv));
        }

        public byte[] DecryptFile(byte[] fileBytes)
        {
            var iv = fileBytes.Take(ivSize).ToArray();
            var dataBytes = fileBytes.Skip(ivSize).ToArray();

            return Encryptor.Decrypt(new EncryptedData(dataBytes, iv), Convert.FromBase64String(SecretAsString()));
        }

        public void DecryptFile(string filePath, string filePathToDecryptedFile)
        {
            File.WriteAllBytes(filePathToDecryptedFile, File.ReadAllBytes(filePath));
        }

        public string DecryptString(string value)
        {
            if (!value.Contains("|"))
                throw new FormatException("value must contain the | character");

            var decryptedData = value.Split('|');

            var result = Encryptor.Decrypt(new EncryptedData(Convert.FromBase64String(decryptedData[0]),
                Convert.FromBase64String(decryptedData[0])), Convert.FromBase64String(SecretAsString()));

            return Encoding.UTF8.GetString(result);
        }

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
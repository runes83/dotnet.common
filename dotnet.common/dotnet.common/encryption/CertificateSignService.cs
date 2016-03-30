using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using dotnet.common.exceptions;
using dotnet.common.hash;
using dotnet.common.misc;

namespace dotnet.common.encryption
{
    public enum HashAlgorithm
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public class CertificateSignService : CertificateBaseService
    {
        public CertificateSignService(X509Certificate2 certificate) : base(certificate)
        {
        }

        public CertificateSignService(string certificatePath, string password) : base(certificatePath, password)
        {
        }

        public CertificateSignService(byte[] certificateDataBytes, string password)
            : base(certificateDataBytes, password)
        {
        }

        public CertificateSignService(string thumbprint, StoreName storeName = StoreName.My,
            StoreLocation storeLocation = StoreLocation.CurrentUser) : base(thumbprint, storeName, storeLocation)
        {
        }

        public byte[] Sign(byte[] bytesToSign, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256)
        {
            if (privateKey == null)
                throw new PrivateKeyNotAvailableException(
                    "The private key is not avaible for the certificate, you need the private key to sign");

            return privateKey.SignData(bytesToSign, CryptoConfig.CreateFromName(hashAlgorithm.ToString().ToUpperInvariant()));
        }

        public string Sign(byte[] bytesToSign, HashAlgorithm hashAlgorithm, ByteEncoding byteEncoding)
        {
            return Sign(bytesToSign, hashAlgorithm).EncodeByteArray(byteEncoding);
        }

        public string Sign(string stringToSign, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256,
            ByteEncoding byteEncoding = ByteEncoding.BASE64, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return Sign(encoding.GetBytes(stringToSign), hashAlgorithm, byteEncoding);
        }

        public bool Verify(byte[] bytesToVerify, byte[] signature, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256)
        {
            return publicKey.VerifyData(bytesToVerify, CryptoConfig.CreateFromName(hashAlgorithm.ToString().ToUpperInvariant()), signature);
        }

        public bool Verify(byte[] bytesToVerify, string signature, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256,
            ByteEncoding byteEncoding = ByteEncoding.BASE64)
        {
            if (byteEncoding == ByteEncoding.HEX)
                signature = signature.ToUpperInvariant();
            else if (byteEncoding == ByteEncoding.hex)
                signature = signature.ToLowerInvariant();

            var signatureBytes = byteEncoding == ByteEncoding.BASE64
                ? Convert.FromBase64String(signature)
                : signature.HexToBytes();


            return Verify(bytesToVerify, signatureBytes, hashAlgorithm);
        }

        public bool Verify(string stringToSign, string signature, HashAlgorithm hashAlgorithm = HashAlgorithm.SHA256,
            ByteEncoding byteEncoding = ByteEncoding.BASE64, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            return Verify(encoding.GetBytes(stringToSign), signature, hashAlgorithm, byteEncoding);
        }
    }
}
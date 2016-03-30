using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using dotnet.common.misc;
using dotnet.common.strings;

namespace dotnet.common.encryption
{
    public abstract class CertificateBaseService : IDisposable
    {
        protected X509Certificate2 certificate;
        protected RSACryptoServiceProvider privateKey;
        protected RSACryptoServiceProvider publicKey;

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificate">The certificate to encrypt with as a X509Certificate2 object</param>
        protected CertificateBaseService(X509Certificate2 certificate)
        {
            this.certificate = certificate;
            if (certificate.HasPrivateKey)
            {
                var privKey = (RSACryptoServiceProvider)certificate.PrivateKey;
                // Force use of the Enhanced RSA and AES Cryptographic Provider with openssl-generated SHA256 keys
                var enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
                var cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, privKey.CspKeyContainerInfo.KeyContainerName);
                privateKey = new RSACryptoServiceProvider(cspparams);
            }
            publicKey = certificate.PublicKey.Key as RSACryptoServiceProvider;
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="certificatePath">Path to the certificate to use</param>
        /// <param name="password">Password to the ceriticate to use</param>
        protected CertificateBaseService(string certificatePath, string password)
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
        protected CertificateBaseService(byte[] certificateDataBytes, string password)
        {
            certificate = new X509Certificate2(certificateDataBytes, password, X509KeyStorageFlags.MachineKeySet);
        }

        /// <summary>
        ///     Initialize the encryptionservice
        /// </summary>
        /// <param name="thumbprint">The thumprint for the ceriticate to use</param>
        /// <param name="storeName">What certificate store to use default My store</param>
        /// <param name="storeLocation">The certificate store location to use default CurrentUser</param>
        protected CertificateBaseService(string thumbprint, StoreName storeName = StoreName.My,
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
            privateKey.DoIfNotNull(x=>x.Dispose());
            publicKey.DoIfNotNull(x=>x.Dispose());
            certificate = null;


        }
    }
}
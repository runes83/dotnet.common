using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using dotnet.common.encryption;
using dotnet.common.hash;
using dotnet.common.security;
using NUnit.Framework;

namespace dotnet.common.test
{
    [TestFixture]
    public class EncryptionTest
    {

        private string secret;
        private X509Certificate2 certificate;

        [TestFixtureSetUp]
        public void Setup()
        {
            secret = EncryptionService.GenerateNewSecret();
            certificate = new X509Certificate2(File.ReadAllBytes(@"TestFiles\TCA.p12"), "test");
        }

        [Test]
        public void TestEncryptAndDecryptShouldBeTheSameFile()
        {
            var dataBytes = File.ReadAllBytes(@"TestFiles\documenttosign.txt");
            var sha1 = dataBytes.ToSha1(HashFormat.BASE64);

            using (var encryptionService = new EncryptionService(secret))
            {
                var encryptedBytes = encryptionService.EncryptFile(dataBytes);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(HashFormat.BASE64)));

                var decryptedBytes = encryptionService.DecryptFile(encryptedBytes);
                var decryptedSha1 = decryptedBytes.ToSha1(HashFormat.BASE64);

                Assert.AreEqual(sha1,decryptedSha1);
            }         
        }



        [Test]
        public void TestEncryptWithCertificateAndDecryptShouldBeTheSameFile()
        {
            var dataBytes = File.ReadAllBytes(@"TestFiles\documenttosign.txt");
            var sha1 = dataBytes.ToSha1(HashFormat.BASE64);

            using (var encryptionService = new CertificateEncryptionService(certificate))
            {
                var encryptedBytes = encryptionService.EncryptFile(dataBytes);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(HashFormat.BASE64)));

                var decryptedBytes = encryptionService.DecryptFile(encryptedBytes);
                var decryptedSha1 = decryptedBytes.ToSha1(HashFormat.BASE64);

                Assert.AreEqual(sha1, decryptedSha1);
            }
        }


        [Test]
        public void GetIVLength()
        {
            for (int i = 0; i < 1; i++)
            {
                using (var rijndael = new AesManaged())
                {
                    rijndael.GenerateIV();
                    rijndael.GenerateKey();
                    rijndael.KeySize = 256;
                    Console.WriteLine("{0} length: {1}",Convert.ToBase64String(rijndael.IV), rijndael.IV.Length);

                    Console.WriteLine("{0} length: {1}", Convert.ToBase64String(rijndael.Key), rijndael.Key.Length);

                }
            }
        }

    }
}

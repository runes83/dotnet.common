using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using dotnet.common.encryption;
using dotnet.common.hash;
using dotnet.common.test.strings;
using NUnit.Framework;

namespace dotnet.common.test.encryption
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

        }

        [SetUp]
        public void SetupEachTime()
        {
            certificate = new X509Certificate2(File.ReadAllBytes(@"TestFiles\TCA.p12"), "test");
        }

        [Test]
        public void TestEncryptAndDecryptShouldBeTheSameFile()
        {
            var dataBytes = File.ReadAllBytes(@"TestFiles\documenttosign.txt");
            var sha1 = dataBytes.ToSha1(ByteEncoding.BASE64);

            using (var encryptionService = new EncryptionService(secret))
            {
                var encryptedBytes = encryptionService.EncryptFile(dataBytes);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(ByteEncoding.BASE64)));

                var decryptedBytes = encryptionService.DecryptFile(encryptedBytes);
                var decryptedSha1 = decryptedBytes.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha1);
            }
        }



        [Test]
        public void TestEncryptWithCertificateAndDecryptShouldBeTheSameFile()
        {
            var dataBytes = File.ReadAllBytes(@"TestFiles\documenttosign.txt");
            var sha1 = dataBytes.ToSha1(ByteEncoding.BASE64);

            using (var encryptionService = new CertificateEncryptionService(certificate))
            {
                var encryptedBytes = encryptionService.EncryptFile(dataBytes);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(ByteEncoding.BASE64)));

                var decryptedBytes = encryptionService.DecryptFile(encryptedBytes);
                var decryptedSha1 = decryptedBytes.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha1);
            }
        }

        [Test]
        public void TestEncryptWithSecretAndDecryptShouldBeTheSameStringShortString()
        {
            var testString = Guid.NewGuid().ToString();
            var sha1 = testString.ToSha1(ByteEncoding.BASE64);

            using (var encryptionService = new EncryptionService(secret))
            {
                var encryptedBytes = encryptionService.EncryptString(testString);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(ByteEncoding.BASE64)));

                var decryptedBytes = encryptionService.DecryptString(encryptedBytes);
                var decryptedSha1 = decryptedBytes.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha1);
            }
        }

        [Test]
        public void TestEncryptWithSecretAndDecryptShouldBeTheSameStringLongString()
        {
            var testString = StringsTests.truncateTestString;
            var sha1 = testString.ToSha1(ByteEncoding.BASE64);

            using (var encryptionService = new EncryptionService(secret))
            {
                var encryptedBytes = encryptionService.EncryptString(testString, ByteEncoding.HEX);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(ByteEncoding.BASE64)));
                Console.WriteLine(encryptedBytes);
                Console.WriteLine("HEX length: {0}", encryptedBytes.Length);
                var decryptedBytes = encryptionService.DecryptString(encryptedBytes, ByteEncoding.HEX);
                var decryptedSha1 = decryptedBytes.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha1);

                var encryptedBytes2 = encryptionService.EncryptString(testString, ByteEncoding.hex);
                Assert.IsFalse(sha1.Equals(encryptedBytes2.ToSha1(ByteEncoding.BASE64)));
                Console.WriteLine(encryptedBytes2);
                Console.WriteLine("hex length: {0}", encryptedBytes2.Length);

                var decryptedBytes2 = encryptionService.DecryptString(encryptedBytes2, ByteEncoding.hex);
                var decryptedSha12 = decryptedBytes2.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha12);

                var encryptedBytes3 = encryptionService.EncryptString(testString, ByteEncoding.BASE64);
                Assert.IsFalse(sha1.Equals(encryptedBytes3.ToSha1(ByteEncoding.BASE64)));
                Console.WriteLine(encryptedBytes3);
                Console.WriteLine("base64 length: {0}", encryptedBytes3.Length);

                var decryptedBytes3 = encryptionService.DecryptString(encryptedBytes3, ByteEncoding.BASE64);
                var decryptedSha13 = decryptedBytes3.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha13);
            }
        }

        [Test]
        public void TestEncryptWithCertificateAndDecryptShouldBeTheSameStringShortString()
        {
            var testString = Guid.NewGuid().ToString();
            var sha1 = testString.ToSha1(ByteEncoding.BASE64);

            using (var encryptionService = new CertificateEncryptionService(certificate))
            {
                var encryptedString = encryptionService.EncryptString(testString);
                Assert.IsFalse(sha1.Equals(encryptedString.ToSha1(ByteEncoding.BASE64)));
                Console.WriteLine(encryptedString);
                var decryptString = encryptionService.DecryptString(encryptedString);
                var decryptedSha1 = decryptString.ToSha1(ByteEncoding.BASE64);

                Assert.AreEqual(sha1, decryptedSha1);
            }
        }

        [Test]
        public void TestEncryptWithCertificateAndDecryptShouldBeTheSameStringLongString()
        {
            var testString = StringsTests.truncateTestString;
            var sha1 = testString.ToSha1(ByteEncoding.BASE64);

            using (var encryptionService = new CertificateEncryptionService(certificate))
            {
                var encryptedBytes = encryptionService.EncryptString(testString);
                Assert.IsFalse(sha1.Equals(encryptedBytes.ToSha1(ByteEncoding.BASE64)));

                var decryptedBytes = encryptionService.DecryptString(encryptedBytes);
                var decryptedSha1 = decryptedBytes.ToSha1(ByteEncoding.BASE64);

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
                    Console.WriteLine("{0} length: {1}", Convert.ToBase64String(rijndael.IV), rijndael.IV.Length);

                    Console.WriteLine("{0} length: {1}", Convert.ToBase64String(rijndael.Key), rijndael.Key.Length);

                }
            }
        }
    }
}

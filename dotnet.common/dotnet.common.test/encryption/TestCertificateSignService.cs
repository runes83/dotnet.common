using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using dotnet.common.encryption;
using dotnet.common.hash;
using NUnit.Framework;

namespace dotnet.common.test.encryption
{
    [TestFixture]
    public class TestCertificateSignService
    {
        private X509Certificate2 certificate;

        [SetUp]
        public void SetupEachTime()
        {
            certificate = new X509Certificate2(File.ReadAllBytes(@"TestFiles\tca.p12"), "test");
        }

        [Test]
        public void Test_Sign_file_with_ceritifcate_and_verify_Should_be_true()
        {
            var dataBytes = File.ReadAllBytes(@"TestFiles\documenttosign.txt");

            using (var signService = new CertificateSignService(certificate))
            {
                var signature = signService.Sign(dataBytes,HashAlgorithm.SHA384);

                Console.WriteLine("{0} length: {1}", Convert.ToBase64String(signature),signature.Length);
                Assert.IsTrue(signService.Verify(dataBytes,signature,HashAlgorithm.SHA384));
            }
        }
    }
}
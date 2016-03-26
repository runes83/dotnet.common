using System;
using System.Security.Cryptography;
using System.Text;

namespace dotnet.common.security
{
    public class RandomStringGenerator : IDisposable
    {
        public enum Characters
        {
            NUMBERS,
            ALFANUMERIC,
            ALFANUMERIC_LOWERCASE,
            ALFANUMERIC_UPPERCASE,
            WITHSPECIALCHARACTERS
        }

        private readonly RNGCryptoServiceProvider Rand;

        public RandomStringGenerator()
        {
            Rand = new RNGCryptoServiceProvider();
        }

        public void Dispose()
        {
            Rand.Dispose();
        }

        public static string GenerateRandomString(int length, Characters characters)
        {
            using (var rand = new RandomStringGenerator())
            {
                return rand.Generate(length, characters);
            }
        }

        public string Generate(int length, params char[] chars)
        {
            var s = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var intBytes = new byte[4];
                Rand.GetBytes(intBytes);
                var randomInt = BitConverter.ToUInt32(intBytes, 0);
                s.Append(chars[randomInt%chars.Length]);
            }
            return s.ToString();
        }

        public string Generate(int length, Characters characters)
        {
            return Generate(length, ReturnValidChars(characters));
        }

        private char[] ReturnValidChars(Characters characters)
        {
            switch (characters)
            {
                case Characters.NUMBERS:
                    return "0123456789".ToCharArray();
                case Characters.ALFANUMERIC_LOWERCASE:
                    return "abcdefghijklmnopqrstuvwzxyz1234567890".ToCharArray();
                case Characters.ALFANUMERIC_UPPERCASE:
                    return "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                case Characters.ALFANUMERIC:
                    return "abcdefghijklmnopqrstuvwzxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                default:
                    return
                        "abcdefghijklmnopqrstuvwzxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"
                            .ToCharArray();
            }
        }
    }
}
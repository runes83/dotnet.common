using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using dotnet.common.strings;
using NUnit.Framework;

namespace dotnet.common.test.strings
{
    [TestFixture]
    public class StringsTests
    {
        public static string truncateTestString =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam tristique lectus elit, dapibus vestibulum lectus accumsan id. Aenean malesuada velit ac mi convallis volutpat. Aenean quis purus ac orci viverra ultricies. Integer fermentum pharetra iaculis. Fusce metus lectus, aliquam interdum luctus non, tempus at erat. Proin tristique erat a dolor posuere, sit amet interdum tellus tempus. Sed vel sagittis tellus. Nullam laoreet magna leo, a vulputate est semper ut. In eu eros eu sem imperdiet ultrices ac at magna. Aliquam molestie aliquam dignissim. Vestibulum lobortis, nisl eget eleifend porttitor, nisi lacus malesuada quam, non malesuada libero erat nec erat. Integer consectetur ultricies arcu. Mauris vitae lacinia elit. Maecenas id odio eu metus dapibus luctus quis in elit. In hac habitasse platea dictumst.";
        [Test]
        public void Test_that_format_with_returns_the_same_as_string_dot_format()
        {
            string test = "Number: {0} bool: {1} and another string: {2}";
            Assert.AreEqual(string.Format(test,1,true,"test"),test.FormatWith(1,true,"test"));
        }

        [Test]
        public void Test_that_format_with_returns_null_when_operating_on_null_string()
        {
            string test = null;
            Assert.IsNull(test.FormatWith(1,2,3));
        }

        [Test]
        public void Test_that_format_with_does_not_throw_exception_when_operating_on_null_string()
        {
            string test = null;
            Assert.DoesNotThrow(()=>test.FormatWith(1,2,3));
        }

        //Truncate
        
        [Test]
        public void Test_that_truncate_returns_correct_length()
        {
            Assert.AreEqual(25,truncateTestString.Truncate(25).Length);
        }

        [Test]
        public void Test_that_truncate_returns_correct_length_with_Dots()
        {
            Assert.AreEqual(25, truncateTestString.Truncate(25,true).Length);
        }

        [Test]
        public void Test_that_truncate_with_Dots_ends_with_three_dots_when_string_is_longer_than_length()
        {
            Assert.IsTrue( truncateTestString.Truncate(25, true).EndsWith("..."));
        }

        [Test]
        public void Test_that_truncate_with_Dots_does_not_ends_with_three_dots_when_string_is_short_than_length()
        {
            Assert.IsFalse(truncateTestString.Truncate(truncateTestString.Length, true).EndsWith("..."));
        }

        [Test]
        public void Test_that_ToBase64String_only_contains_base64_characters()
        {
            string url = "https://github.com/runes83/dotnet.common";
            string result = url.ToBase64String();

            Assert.IsTrue(Regex.IsMatch(result, "^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$"));
        }

        [Test]
        public void Test_that_ToBase64String_and_FromBase64_returns_same_string()
        {
            string url = "https://github.com/runes83/dotnet.common";

            Assert.AreEqual(url,url.ToBase64String().FromBase64String());

            Assert.AreEqual(truncateTestString,truncateTestString.ToBase64String().FromBase64String());
        }

        [Test]
        public void Test_that_to_secure_string_return_secure_string_object_that_is_not_null()
        {
            var result = Guid.NewGuid().ToString().ToSecureString();
            Assert.IsNotNull(result);
            Assert.IsTrue(result is SecureString);
           
        }

        [Test]
        public void Test_that_to_secure_string_and_from_secure_string_returns_same_value()
        {
            string testValue = Guid.NewGuid().ToString();
            var secureString = testValue.ToSecureString();
            Assert.IsNotNull(secureString);
            Assert.IsTrue(secureString is SecureString);

            Assert.AreEqual(testValue,secureString.ToUnsecureString());

        }
    }
}

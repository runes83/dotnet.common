using System;
using System.Linq;
using dotnet.common.reflection;
using NUnit.Framework;

namespace dotnet.common.test.reflection
{
    [TestFixture]
    public class ReflectionTests
    {
        [Test]
        public void Test_Get_All_Properties_From_Object()
        {
            var testObj = new ReflectionTestObject
            {
                Test1 = Guid.NewGuid().ToString("n"),
                Test2 = true,
                Test3 = new Random().Next(),
                Test4 = new[] {"Test1", "Test2"}
            };


            var list = testObj.ListProperties();

            Assert.IsTrue(list != null && list.Any());
            Assert.AreEqual(4, list.Count);

            Assert.AreEqual(list["Test1"], testObj.Test1);
            Assert.AreEqual(list["Test2"], testObj.Test2);
            Assert.AreEqual(list["Test3"], testObj.Test3);
            Assert.AreEqual((list["Test4"] as string[]).Count(), testObj.Test4.Count());

            Assert.AreEqual((list["Test4"] as string[]).First(), testObj.Test4.First());
            Assert.AreEqual((list["Test4"] as string[]).Last(), testObj.Test4.Last());
        }

        [Test]
        public void Test_Get_Property_Value_On_Property_That_Does_Not_Exist()
        {
            var testObj = new ReflectionTestObject {Test1 = Guid.NewGuid().ToString("n")};

            Assert.IsNull(testObj.GetByName("Test"));

            var ex = Assert.Throws<Exception>(() => testObj.GetByName("Test", true));
            Assert.AreEqual(ex.Message, "Property does not exist with name: Test");
        }

        [Test]
        public void Test_Get_Property_Value_On_Property_That_Exist()
        {
            var testObj = new ReflectionTestObject
            {
                Test1 = Guid.NewGuid().ToString("n"),
                Test2 = true,
                Test3 = new Random().Next(),
                Test4 = new[] {"Test1", "Test2"}
            };

            string test1Value = testObj.GetByName("Test1");
            bool test2Value = testObj.GetByName("Test2");
            int test3Value = testObj.GetByName("Test3");
            string[] test4Value = testObj.GetByName("Test4");

            Assert.AreEqual(test1Value, testObj.Test1);
            Assert.AreEqual(test2Value, testObj.Test2);
            Assert.AreEqual(test3Value, testObj.Test3);
            Assert.AreEqual(test4Value.Count(), testObj.Test4.Count());

            Assert.AreEqual(test4Value.First(), testObj.Test4.First());
            Assert.AreEqual(test4Value.Last(), testObj.Test4.Last());
        }

        [Test]
        public void Test_Set_Property_Value_On_Property_That_Does_Not_Exist()
        {
            var testObj = new ReflectionTestObject();
            var test1Value = Guid.NewGuid().ToString("n");


            Assert.DoesNotThrow(() => testObj.SetByName("Test", test1Value));

            var ex = Assert.Throws<Exception>(() => testObj.SetByName("Test", test1Value, true));
            Assert.AreEqual(ex.Message, "Property does not exist with name: Test");
        }

        [Test]
        public void TestSetPropertyValueOnPropertyThatExist()
        {
            var testObj = new ReflectionTestObject();
            var test1Value = Guid.NewGuid().ToString("n");
            var test2Value = true;
            var test3Value = new Random().Next();
            var test4Value = new[] {"Test1", "Test2"};

            testObj.SetByName("Test1", test1Value);
            testObj.SetByName("Test2", test2Value);
            testObj.SetByName("Test3", test3Value);
            testObj.SetByName("Test4", test4Value);

            Assert.AreEqual(test1Value, testObj.Test1);
            Assert.AreEqual(test2Value, testObj.Test2);
            Assert.AreEqual(test3Value, testObj.Test3);
            Assert.AreEqual(test4Value.Count(), testObj.Test4.Count());

            Assert.AreEqual(test4Value.First(), testObj.Test4.First());
            Assert.AreEqual(test4Value.Last(), testObj.Test4.Last());
        }
    }
}
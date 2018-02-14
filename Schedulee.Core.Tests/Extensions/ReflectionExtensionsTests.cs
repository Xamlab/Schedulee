using NUnit.Framework;
using Schedulee.Core.Extensions;
using Shouldly;

namespace Schedulee.Core.Tests.Extensions
{
    [TestFixture]
    public class ReflectionExtensionsTests
    {
        private class TestClass
        {
            public string TestProperty { get; set; }
        }

        [Test]
        public void TestGetPropertyValue()
        {
            var testValue = "Test";
            var testObject = new TestClass {TestProperty = testValue};
            var propertyValue = testObject.GetPropertyValue(nameof(TestClass.TestProperty));
            propertyValue.ShouldBe(testValue);
        }
    }
}
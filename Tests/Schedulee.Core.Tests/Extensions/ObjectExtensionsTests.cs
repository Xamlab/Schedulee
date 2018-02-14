using NUnit.Framework;
using Schedulee.Core.Extensions;
using Shouldly;

namespace Schedulee.Core.Tests.Extensions
{
    [TestFixture]
    public class MiscTests
    {
        [Test]
        public void TestObjectsEqual()
        {
            object obj1 = null;
            object obj2 = null;
            obj1.ObjectsEqual(obj2).ShouldBe(true);

            var value1 = "value";
            var value2 = "value";
            value1.ObjectsEqual(value2).ShouldBe(true);

            obj2 = new object();
            obj1.ObjectsEqual(obj2).ShouldBe(false);

            value1 = "value1";
            value2 = "value2";
            value1.ObjectsEqual(value2).ShouldBe(false);
        }
    }
}
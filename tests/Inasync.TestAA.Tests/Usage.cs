using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class Usage {
        private static TestAssert _defaultTestAssert;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            _defaultTestAssert = TestAA.TestAssert;
            TestAA.TestAssert = new MSTestAssert();
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            TestAA.TestAssert = _defaultTestAssert;
        }

        private sealed class MSTestAssert : TestAssert {

            public override void Is<T>(T actual, T expected, string message) => Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        public void Basic() {
            TestAA.Act(() => int.Parse("123")).Assert(123);
        }

        [TestMethod]
        public void Exception() {
            TestAA.Act(() => int.Parse("abc")).Assert(ret => { }, exception: new FormatException());
        }

        [TestMethod]
        public void AssertOutParameter() {
            int result = default;
            TestAA.Act(() => int.TryParse("123", out result)).Assert(true);

            // Additional Assert
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void CustomAssert() {
            TestAA.Act(() => int.Parse("123")).Assert(
                  @return: ret => Assert.AreEqual(123, ret)
                , exception: ex => Assert.IsNull(ex)
            );
        }

        [TestMethod]
        public void TestCases() {
            Action TestCase(int testNumber, string input, int expected, Exception expectedException = null) => () => {
                var msg = "No." + testNumber;

                TestAA.Act(() => int.Parse(input)).Assert(expected, expectedException, msg);
            };

            foreach (var action in new[] {
                TestCase( 0, null , expected: 0  , expectedException: new ArgumentNullException()),
                TestCase( 1, "abc", expected: 0  , expectedException: new FormatException()),
                TestCase( 2, "123", expected: 123),
            }) { action(); }
        }
    }
}

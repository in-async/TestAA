using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class Usage {

        [TestMethod]
        public void Basic() {
            Action TestCase(int testNumber, string input, int expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                TestAA.Act(() => int.Parse(input)).Assert(
                    ret => Assert.AreEqual(expected, ret, msg),
                    ex => Assert.AreEqual(expectedExceptionType, ex?.GetType(), msg)
                );
            };

            foreach (var action in new[] {
                TestCase( 0, null , expected: 0  , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, "abc", expected: 0  , expectedExceptionType: typeof(FormatException)),
                TestCase( 2, "123", expected: 123),
            }) { action(); }
        }
    }
}

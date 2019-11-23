using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestAssert_Tests {

        [TestMethod]
        public void Is() {
            Action TestCase<T>(int testNumber, T actual, T expected, string message, string expectedMessage) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception = null;
                try {
                    TestAssert.Default.Is(actual, expected, message);
                }
                catch (Exception ex) {
                    exception = ex;
                }

                // Assert
                Assert.AreEqual(expectedMessage, exception?.Message, msg);
            };

            foreach (var action in new[] {
                TestCase( 0, actual:"A" , expected:"A" , message:"M" , expectedMessage: null),
                TestCase( 1, actual:"A" , expected:"B" , message:"M" , expectedMessage: string.Format("Assert failed.{0}Expected: <B>{0}Actual: <A>{0}M", Environment.NewLine)),
                TestCase( 2, actual:"A" , expected:"B" , message:null, expectedMessage: string.Format("Assert failed.{0}Expected: <B>{0}Actual: <A>{0}", Environment.NewLine)),
                TestCase( 3, actual:"A" , expected:null, message:"M" , expectedMessage: string.Format("Assert failed.{0}Expected: (null){0}Actual: <A>{0}M", Environment.NewLine)),
                TestCase( 4, actual:null, expected:"B" , message:"M" , expectedMessage: string.Format("Assert failed.{0}Expected: <B>{0}Actual: (null){0}M", Environment.NewLine)),
            }) { action(); }
        }

        [TestMethod]
        public void AssertException() {
            Action TestCase(int testNumber, Exception actual, Type expectedType, string message, string expectedMessage) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception = null;
                try {
                    TestAssert.Default.AssertException(actual, expectedType, message);
                }
                catch (Exception ex) {
                    exception = ex;
                }

                // Assert
                Assert.AreEqual(expectedMessage, exception?.Message, msg);
            };

            var exA = new ApplicationException();
            foreach (var action in new[] {
                TestCase( 0, actual:exA , expectedType:typeof(ApplicationException), message:"M" , expectedMessage: null),
                TestCase( 1, actual:exA , expectedType:typeof(Exception)           , message:"M" , expectedMessage: string.Format("Assert failed.{0}Expected: <System.Exception>{0}Actual: <System.ApplicationException>{0}M{0}Actual Exception: " + exA, Environment.NewLine)),
                TestCase( 2, actual:exA , expectedType:typeof(Exception)           , message:null, expectedMessage: string.Format("Assert failed.{0}Expected: <System.Exception>{0}Actual: <System.ApplicationException>{0}{0}Actual Exception: " + exA, Environment.NewLine)),
                TestCase( 3, actual:exA , expectedType:null                        , message:"M" , expectedMessage: string.Format("Assert failed.{0}Expected: (null){0}Actual: <System.ApplicationException>{0}M{0}Actual Exception: " + exA, Environment.NewLine)),
                TestCase( 4, actual:null, expectedType:typeof(Exception)           , message:"M" , expectedMessage: string.Format("Assert failed.{0}Expected: <System.Exception>{0}Actual: (null){0}M", Environment.NewLine)),
            }) { action(); }
        }

        [TestMethod]
        public void TestAA_TestAssert_get() {
            // Act
            var @return = TestAA.TestAssert;

            // Assert
            Assert.AreSame(TestAssert.Default, @return);
        }

        [TestMethod]
        public void TestAA_TestAssert_set() {
            Action TestCase(int testNumber, TestAssert testAssert, TestAssert expected) => () => {
                var defaultTestAssert = TestAA.TestAssert;
                try {
                    // Act
                    TestAA.TestAssert = testAssert;

                    // Assert
                    Assert.AreSame(expected, TestAA.TestAssert);
                }
                finally {
                    TestAA.TestAssert = defaultTestAssert;
                }
            };

            var dummy = new DummyTestAssert();
            foreach (var action in new[] {
                TestCase( 0, null , TestAssert.Default),
                TestCase( 1, dummy, dummy),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyTestAssert : TestAssert { }

        #endregion Helpers
    }
}

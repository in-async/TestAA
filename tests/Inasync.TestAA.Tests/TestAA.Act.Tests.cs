using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestAA_Act_Tests {

        [TestMethod]
        public void Act() {
            Action TestCase(int testNumber, Action act, Type exceptionType, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                TestActual testActual = default;
                Exception exception = null;
                try {
                    testActual = TestAA.Act(act);
                }
                catch (Exception ex) {
                    exception = ex;
                }

                // Assert
                if (exception == null) {
                    Assert.AreEqual(typeof(TestActual), testActual.GetType(), msg);
                    Assert.AreEqual(exceptionType, testActual.Exception?.GetType(), msg);
                }
                Assert.AreEqual(expectedExceptionType, exception?.GetType(), msg);
            };

            foreach (var action in new[]{
                TestCase( 0, act: null                            , exceptionType: null                  , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => { }                       , exceptionType: null                  ),
                TestCase( 2, act: () => throw new DummyException(), exceptionType: typeof(DummyException)),
            }) { action(); }
        }

        [TestMethod]
        public void Act_TReturn() {
            Action TestCase(int testNumber, Func<DummyObject> act, (DummyObject @return, Type exceptionType) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                TestActual<DummyObject> testActual = default;
                Exception exception = null;
                try {
                    testActual = TestAA.Act(act);
                }
                catch (Exception ex) {
                    exception = ex;
                }

                // Assert
                if (exception == null) {
                    Assert.AreEqual(expected.exceptionType, testActual.Exception?.GetType(), msg);
                    Assert.AreEqual(expected.@return, testActual.Return, msg);
                }
                Assert.AreEqual(expectedExceptionType, exception?.GetType(), msg);
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, act: null                            , expected: default                       , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => obj                       , expected: (obj , null)                  ),
                TestCase( 2, act: () => throw new DummyException(), expected: (null, typeof(DummyException))),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        #endregion Helpers
    }
}

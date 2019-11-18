using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestActual_Assert_Tests {

        [TestMethod]
        public void Assert_ActionException() {
            Action TestCase(int testNumber, TestActual testActual, AssertException assertException, Exception expectedAssertException, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(assertException?.Invoke);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
                Assert.AreEqual(expectedAssertException, assertException?.InvokedParams, msg);
            };

            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 0, new TestActual(null), null                 , expectedAssertException: null, expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, new TestActual(null), new AssertException(), expectedAssertException: null),
                TestCase( 2, new TestActual(ex)  , null                 , expectedAssertException: null, expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 3, new TestActual(ex)  , new AssertException(), expectedAssertException: ex  ),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_Exception_Message() {
            Action TestCase(int testNumber, TestActual testActual, Exception exception, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(exception);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 0, new TestActual(null), null),
                TestCase( 1, new TestActual(null), ex  , expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 2, new TestActual(ex)  , null, expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 3, new TestActual(ex)  , ex  ),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        private sealed class AssertReturn {
            public DummyObject InvokedParams { get; private set; }

            public Action<DummyObject> Invoke => @return => InvokedParams = @return;
        }

        private sealed class AssertException {
            public Exception InvokedParams { get; private set; }

            public Action<Exception> Invoke => ex => InvokedParams = ex;
        }

        #endregion Helpers
    }

    [TestClass]
    public class TestActual_TReturn_Assert_Tests {

        [TestMethod]
        public void Assert_ActionTReturn_ActionException() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, AssertReturn @return, AssertException assertException, (DummyObject @return, Exception assertException) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(@return?.Invoke, assertException?.Invoke);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
                Assert.AreEqual(expected.@return, @return?.InvokedParams, msg);
                Assert.AreEqual(expected.assertException, assertException?.InvokedParams, msg);
            };

            var obj = new DummyObject();
            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 0, new TestActual<DummyObject>(obj), null              , new AssertException(), expected: (@return: null, assertException: null), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, new TestActual<DummyObject>(obj), new AssertReturn(), null                 , expected: (@return: null, assertException: null), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 2, new TestActual<DummyObject>(obj), new AssertReturn(), new AssertException(), expected: (@return: obj , assertException: null)),
                TestCase( 3, new TestActual<DummyObject>(ex ), null              , new AssertException(), expected: (@return: null, assertException: null), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 4, new TestActual<DummyObject>(ex ), new AssertReturn(), null                 , expected: (@return: null, assertException: null), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 5, new TestActual<DummyObject>(ex ), new AssertReturn(), new AssertException(), expected: (@return: null, assertException: ex  )),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_ActionTReturn_Exception_Message() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, AssertReturn @return, Exception exception, DummyObject expectedReturn, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(@return?.Invoke, exception);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
                Assert.AreEqual(expectedReturn, @return?.InvokedParams, msg);
            };

            var obj = new DummyObject();
            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 0, new TestActual<DummyObject>(obj), null              , exception: ex  , expectedReturn: null, expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, new TestActual<DummyObject>(obj), new AssertReturn(), exception: null, expectedReturn: obj ),
                TestCase( 2, new TestActual<DummyObject>(obj), new AssertReturn(), exception: ex  , expectedReturn: null, expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 3, new TestActual<DummyObject>(ex ), null              , exception: ex  , expectedReturn: null, expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 4, new TestActual<DummyObject>(ex ), new AssertReturn(), exception: null, expectedReturn: null, expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 5, new TestActual<DummyObject>(ex ), new AssertReturn(), exception: ex  , expectedReturn: null),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_TReturn_Exception_Message() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, DummyObject @return, Exception exception, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(@return, exception);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            var obj = new DummyObject();
            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 0, new TestActual<DummyObject>(obj), @return: null, exception: ex  , expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 1, new TestActual<DummyObject>(obj), @return: obj , exception: null),
                TestCase( 2, new TestActual<DummyObject>(obj), @return: obj , exception: ex  , expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 3, new TestActual<DummyObject>(ex ), @return: null, exception: ex  ),
                TestCase( 4, new TestActual<DummyObject>(ex ), @return: obj , exception: null, expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 5, new TestActual<DummyObject>(ex ), @return: obj , exception: ex  ),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        private sealed class AssertReturn {
            public DummyObject InvokedParams { get; private set; }

            public Action<DummyObject> Invoke => @return => InvokedParams = @return;
        }

        private sealed class AssertException {
            public Exception InvokedParams { get; private set; }

            public Action<Exception> Invoke => ex => InvokedParams = ex;
        }

        #endregion Helpers
    }
}

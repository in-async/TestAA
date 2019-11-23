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
            Action TestCase(int testNumber, TestActual testActual, Type exceptionType, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(exceptionType);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            foreach (var action in new[]{
                TestCase( 0, new TestActual(null)                , null                 ),
                TestCase( 1, new TestActual(null)                , typeof(DummyException), expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 2, new TestActual(new DummyException()), null                  , expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 3, new TestActual(new DummyException()), typeof(DummyException)),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_TException_Message() {
            Action TestCase<TException>(int testNumber, TestActual testActual, Type expectedExceptionType = null) where TException : Exception => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert<TException>();
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            foreach (var action in new[]{
                TestCase<DummyException>( 0, new TestActual(null)                , expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase<DummyException>( 1, new TestActual(new DummyException())),
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
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, AssertReturn @return, Type exceptionType, DummyObject expectedReturn, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(@return?.Invoke, exceptionType);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
                Assert.AreEqual(expectedReturn, @return?.InvokedParams, msg);
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, new TestActual<DummyObject>(obj                 ), null              , exceptionType: typeof(DummyException), expectedReturn: null, expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, new TestActual<DummyObject>(obj                 ), new AssertReturn(), exceptionType: null                  , expectedReturn: obj ),
                TestCase( 2, new TestActual<DummyObject>(obj                 ), new AssertReturn(), exceptionType: typeof(DummyException), expectedReturn: null, expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 3, new TestActual<DummyObject>(new DummyException()), null              , exceptionType: typeof(DummyException), expectedReturn: null, expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 4, new TestActual<DummyObject>(new DummyException()), new AssertReturn(), exceptionType: null                  , expectedReturn: null, expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 5, new TestActual<DummyObject>(new DummyException()), new AssertReturn(), exceptionType: typeof(DummyException), expectedReturn: null),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_TReturn_Exception_Message() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, DummyObject @return, Type exceptionType, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(@return, exceptionType);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, new TestActual<DummyObject>(obj                 ), @return: null, exceptionType: typeof(DummyException), expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 1, new TestActual<DummyObject>(obj                 ), @return: obj , exceptionType: null                 ),
                TestCase( 2, new TestActual<DummyObject>(obj                 ), @return: obj , exceptionType: typeof(DummyException), expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 3, new TestActual<DummyObject>(new DummyException()), @return: null, exceptionType: typeof(DummyException)),
                TestCase( 4, new TestActual<DummyObject>(new DummyException()), @return: obj , exceptionType: null                  , expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 5, new TestActual<DummyObject>(new DummyException()), @return: obj , exceptionType: typeof(DummyException)),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_Exception_Message() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, Type exceptionType, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert(exceptionType);
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, new TestActual<DummyObject>(obj                 ), exceptionType: typeof(DummyException), expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase( 1, new TestActual<DummyObject>(obj                 ), exceptionType: null                 ),
                TestCase( 2, new TestActual<DummyObject>(new DummyException()), exceptionType: typeof(DummyException)),
                TestCase( 3, new TestActual<DummyObject>(new DummyException()), exceptionType: null                  , expectedExceptionType: typeof(TestAssertFailedException)),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_TException_Message() {
            Action TestCase<TException>(int testNumber, TestActual<DummyObject> testActual, Type expectedExceptionType = null) where TException : Exception => () => {
                var msg = "No." + testNumber;

                // Act
                Exception exception_ = null;
                try {
                    testActual.Assert<TException>();
                }
                catch (Exception ex_) {
                    exception_ = ex_;
                }

                // Assert
                Assert.AreEqual(expectedExceptionType, exception_?.GetType(), msg);
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase<DummyException>( 0, new TestActual<DummyObject>(obj                 ), expectedExceptionType: typeof(TestAssertFailedException)),
                TestCase<DummyException>( 1, new TestActual<DummyObject>(new DummyException())),
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

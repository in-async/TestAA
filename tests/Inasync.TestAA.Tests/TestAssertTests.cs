using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestAssertTests {

        [TestMethod]
        public void Assert_() {
            Action TestCase(int testNumber, TestActual testActual, AssertException exception, AssertOthers others, (Exception exception, bool others) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                try {
                    TestAA.Assert(testActual, exception?.Invoke, others?.Invoke);
                }
                catch (Exception ex_) {
                    Assert.AreEqual(expectedExceptionType, ex_.GetType(), msg);
                }

                Assert.AreEqual(expected.exception, exception?.InvokedParams, msg);
                Assert.AreEqual(expected.others, others?.Invoked ?? default, msg);
            };

            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 1, new TestActual(null), new AssertException(), new AssertOthers(), expected: (exception: null, others: true )),
                TestCase( 2, new TestActual(null), null                 , new AssertOthers(), expected: (exception: null, others: false), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 3, new TestActual(null), new AssertException(), null              , expected: (exception: null, others: false)),
                TestCase( 4, new TestActual(ex)  , new AssertException(), new AssertOthers(), expected: (exception: ex  , others: true )),
            }) { action(); }
        }

        [TestMethod]
        public void Assert_TReturn() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, AssertReturn @return, AssertException exception, AssertOthers others, (DummyObject @return, Exception exception, bool others) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                try {
                    TestAA.Assert(testActual, @return?.Invoke, exception?.Invoke, others?.Invoke);
                }
                catch (Exception ex_) {
                    Assert.AreEqual(expectedExceptionType, ex_.GetType(), msg);
                }

                Assert.AreEqual(expected.@return, @return?.InvokedParams, msg);
                Assert.AreEqual(expected.exception, exception?.InvokedParams, msg);
                Assert.AreEqual(expected.others, others?.Invoked ?? default, msg);
            };

            var obj = new DummyObject();
            var ex = new DummyException();
            foreach (var action in new[]{
                TestCase( 1, new TestActual<DummyObject>(obj , null), null              , new AssertException(), new AssertOthers(), expected: (@return: null, exception: null, others: false), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 2, new TestActual<DummyObject>(obj , null), new AssertReturn(), null                 , new AssertOthers(), expected: (@return: null, exception: null, others: false), expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 2, new TestActual<DummyObject>(obj , null), new AssertReturn(), new AssertException(), null              , expected: (@return: obj , exception: null, others: false)),
                TestCase(11, new TestActual<DummyObject>(obj , null), new AssertReturn(), new AssertException(), new AssertOthers(), expected: (@return: obj , exception: null, others: true )),
                TestCase(13, new TestActual<DummyObject>(null, ex  ), new AssertReturn(), new AssertException(), new AssertOthers(), expected: (@return: null, exception: ex  , others: true )),
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

        private sealed class AssertOthers {
            public bool Invoked { get; private set; }

            public Action Invoke => () => Invoked = true;
        }

        #endregion Helpers
    }
}

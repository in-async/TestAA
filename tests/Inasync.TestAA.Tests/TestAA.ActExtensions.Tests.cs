using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestAA_ActExtensions_Tests {

        [TestMethod]
        public void Act_Task() {
            Action TestCase(int testNumber, Func<Task> act, Type exceptionType, Type expectedExceptionType = null) => () => {
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
                TestCase( 1, act: () => Task.CompletedTask        , exceptionType: null                  ),
                TestCase( 2, act: () => throw new DummyException(), exceptionType: typeof(DummyException)),
            }) { action(); }
        }

        [TestMethod]
        public void Act_TaskTReturn() {
            Action TestCase(int testNumber, Func<Task<DummyObject>> act, (DummyObject @return, Type exceptionType) expected, Type expectedExceptionType = null) => () => {
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
                TestCase( 1, act: () => Task.FromResult(obj)      , expected: (obj , null)                  ),
                TestCase( 2, act: () => throw new DummyException(), expected: (null, typeof(DummyException))),
            }) { action(); }
        }

        [TestMethod]
        public void Act_IEnumerable() {
            Action TestCase(int testNumber, Func<IEnumerable<DummyObject>> act, (DummyObject[] @return, Type exceptionType) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                TestActual<DummyObject[]> testActual = default;
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
                    CollectionAssert.AreEqual(expected.@return, testActual.Return, msg);
                }
                Assert.AreEqual(expectedExceptionType, exception?.GetType(), msg);
            };

            var obj1 = new DummyObject();
            var obj2 = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, act: null                            , expected: default                                      , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => new[]{ obj1, obj2 }       , expected: (new[]{ obj1, obj2 }, null)                  ),
                TestCase( 2, act: () => throw new DummyException(), expected: (null               , typeof(DummyException))),
            }) { action(); }
        }

        [TestMethod]
        public void Act_Task_IEnumerable() {
            Action TestCase(int testNumber, Func<Task<IEnumerable<DummyObject>>> act, (DummyObject[] @return, Type exceptionType) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                // Act
                TestActual<DummyObject[]> testActual = default;
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
                    CollectionAssert.AreEqual(expected.@return, testActual.Return, msg);
                }
                Assert.AreEqual(expectedExceptionType, exception?.GetType(), msg);
            };

            var obj1 = new DummyObject();
            var obj2 = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, act: null                                                                , expected: default                                      , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => Task.FromResult<IEnumerable<DummyObject>>(new[]{ obj1, obj2 }), expected: (new[]{ obj1, obj2 }, null)                  ),
                TestCase( 2, act: () => throw new DummyException()                                    , expected: (null               , typeof(DummyException))),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        #endregion Helpers
    }
}

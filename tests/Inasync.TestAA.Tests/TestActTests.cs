using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestActTests {

        [TestMethod]
        public void Run() {
            Action TestCase(int testNumber, Action act, Type exceptionType, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                try {
                    var actual = TestAA.Act(act);

                    Assert.AreEqual(exceptionType, actual.Exception?.GetType(), msg);
                }
                catch (Exception ex) {
                    Assert.AreEqual(expectedExceptionType, ex.GetType(), msg);
                }
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

                try {
                    var actual = TestAA.Act(act);

                    Assert.AreEqual(expected.exceptionType, actual.Exception?.GetType(), msg);
                    Assert.AreEqual(expected.@return, actual.Return, msg);
                }
                catch (Exception ex) {
                    Assert.AreEqual(expectedExceptionType, ex.GetType(), msg);
                }
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, act: null                            , expected: default                       , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => obj                       , expected: (obj , null)                  ),
                TestCase( 2, act: () => throw new DummyException(), expected: (null, typeof(DummyException))),
            }) { action(); }
        }

        [TestMethod]
        public void ActAsync() {
            Action TestCase(int testNumber, Func<Task> act, Type expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                try {
                    var actual = TestAA.ActAsync(act).Result;

                    Assert.AreEqual(expected, actual.Exception?.GetType(), msg);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    Assert.AreEqual(expectedExceptionType, ex.GetType(), msg);
                }
            };

            foreach (var action in new[]{
                TestCase( 0, act: null                            , expected: null                  , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => Task.CompletedTask        , expected: null                  ),
                TestCase( 2, act: () => throw new DummyException(), expected: typeof(DummyException)),
            }) { action(); }
        }

        [TestMethod]
        public void ActAsync_TReturn() {
            Action TestCase(int testNumber, Func<Task<DummyObject>> act, (DummyObject @return, Type exceptionType) expected, Type expectedExceptionType = null) => () => {
                var msg = "No." + testNumber;

                try {
                    var actual = TestAA.ActAsync(act).Result;

                    Assert.AreEqual(expected.exceptionType, actual.Exception?.GetType(), msg);
                    Assert.AreEqual(expected.@return, actual.Return, msg);
                }
                catch (Exception ex) {
                    Assert.AreEqual(expectedExceptionType, ex.GetType(), msg);
                }
            };

            var obj = new DummyObject();
            foreach (var action in new[]{
                TestCase( 0, act: null                            , expected: default                       , expectedExceptionType: typeof(ArgumentNullException)),
                TestCase( 1, act: () => Task.FromResult(obj)      , expected: (obj , null)                  ),
                TestCase( 2, act: () => throw new DummyException(), expected: (null, typeof(DummyException))),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        #endregion Helpers
    }
}

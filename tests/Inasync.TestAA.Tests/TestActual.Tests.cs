using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestActual_Tests {

        [TestMethod]
        public void Ctor() {
            // Act
            var ret = new TestActual();

            // Assert
            Assert.IsNull(ret.Exception);
        }

        [TestMethod]
        public void Ctor_Args() {
            Action TestCase(int testNumber, Exception exception) => () => {
                var msg = "No." + testNumber;

                // Act
                var ret = new TestActual(exception);

                // Assert
                Assert.AreEqual(exception, ret.Exception, msg);
            };

            foreach (var action in new[] {
                TestCase( 0, null),
                TestCase( 1, new DummyException()),
            }) { action(); }
        }

        [TestMethod]
        public void Equals() {
            Action TestCase(int testNumber, TestActual testActual, TestActual other, bool expected) => () => {
                Assert.AreEqual(expected, testActual.Equals(other)
                    , message: $"No.{testNumber}: Equals"
                );

                Assert.AreEqual(expected, testActual.Equals((object)other)
                    , message: $"No.{testNumber}: Equals(object)"
                );

                // 全く関係のない object との比較なので、常に false。
                Assert.AreEqual(false, testActual.Equals(new object())
                    , message: $"No.{testNumber}: Equals(new object)"
                );

                Assert.AreEqual(expected, testActual == other
                    , message: $"No.{testNumber}: =="
                );

                Assert.AreEqual(!expected, testActual != other
                    , message: $"No.{testNumber}: !="
                );

                Assert.AreEqual(expected, ((object)testActual).Equals((object)other)
                    , message: $"No.{testNumber}: object.Equals(object)"
                );

                // 固有型の == 演算子オーバーロードが機能しないので、常に false。
                Assert.AreEqual(false, (object)testActual == (object)other
                    , message: $"No.{testNumber}: object == object"
                );
            };

            var ex = new DummyException();
            foreach (var action in new[] {
                TestCase( 0, new TestActual()  , new TestActual()                    , true ),
                TestCase( 1, new TestActual(ex), new TestActual(ex)                  , true ),
                TestCase( 2, new TestActual(ex), new TestActual(new DummyException()), false),
            }) { action(); }
        }

        [TestMethod]
        public new void GetHashCode() {
            Action TestCase(int testNumber, DummyException exception) => () => {
                var testActual = new TestActual(exception);

                // GetHashCode() は環境によって戻り値が変わるので、例外が起こらない事だけ確認。
                testActual.GetHashCode();
            };

            foreach (var action in new[] {
                TestCase( 0, null),
                TestCase( 1, new DummyException()),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        #endregion Helpers
    }

    [TestClass]
    public class TestActual_TReturnTests {

        [TestMethod]
        public void Ctor() {
            var ret = new TestActual<DummyObject>();

            Assert.IsNull(ret.Return);
            Assert.IsNull(ret.Exception);
        }

        [TestMethod]
        public void Ctor_Args() {
            Action TestCase(int testNumber, DummyObject @return, Exception exception, (DummyObject @return, Exception exception) expected) => () => {
                var msg = "No." + testNumber;

                var ret = new TestActual<DummyObject>(@return, exception);

                Assert.AreEqual(expected.@return, ret.Return, msg);
                Assert.AreEqual(expected.exception, ret.Exception, msg);
            };

            var obj = new DummyObject();
            var ex = new DummyException();
            foreach (var action in new[] {
                TestCase( 0, obj , ex  , (@return: null, exception: ex)),
                TestCase( 1, null, ex  , (@return: null, exception: ex)),
                TestCase( 2, obj , null, (@return: obj , exception: null)),
            }) { action(); }
        }

        [TestMethod]
        public void Equals() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual, TestActual<DummyObject> other, bool expected) => () => {
                Assert.AreEqual(expected, testActual.Equals(other)
                    , message: $"No.{testNumber}: Equals"
                );

                Assert.AreEqual(expected, testActual.Equals((object)other)
                    , message: $"No.{testNumber}: Equals(object)"
                );

                // 全く関係のない object との比較なので、常に false。
                Assert.AreEqual(false, testActual.Equals(new object())
                    , message: $"No.{testNumber}: Equals(new object)"
                );

                Assert.AreEqual(expected, testActual == other
                    , message: $"No.{testNumber}: =="
                );

                Assert.AreEqual(!expected, testActual != other
                    , message: $"No.{testNumber}: !="
                );

                Assert.AreEqual(expected, ((object)testActual).Equals((object)other)
                    , message: $"No.{testNumber}: object.Equals(object)"
                );

                // AbmPlacementId の == 演算子オーバーロードが機能しないので、常に false。
                Assert.AreEqual(false, (object)testActual == (object)other
                    , message: $"No.{testNumber}: object == object"
                );
            };

            var obj = new DummyObject();
            var ex = new DummyException();
            foreach (var action in new[] {
                TestCase( 0, new TestActual<DummyObject>()       , new TestActual<DummyObject>()                                       , true ),
                TestCase( 1, new TestActual<DummyObject>(obj, ex), new TestActual<DummyObject>(obj              , ex                  ), true ),
                TestCase( 2, new TestActual<DummyObject>(obj, ex), new TestActual<DummyObject>(new DummyObject(), ex                  ), true ),
                TestCase( 3, new TestActual<DummyObject>(obj, ex), new TestActual<DummyObject>(obj              , new DummyException()), false),
            }) { action(); }
        }

        [TestMethod]
        public new void GetHashCode() {
            Action TestCase(int testNumber, DummyObject @return, DummyException exception) => () => {
                var testActual = new TestActual<DummyObject>(@return, exception);

                // GetHashCode() は環境によって戻り値が変わるので、例外が起こらない事だけ確認。
                testActual.GetHashCode();
            };

            foreach (var action in new[] {
                TestCase( 0, new DummyObject(), null),
                TestCase( 1, null             , new DummyException()),
                TestCase( 2, new DummyObject(), new DummyException()),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        #endregion Helpers
    }
}

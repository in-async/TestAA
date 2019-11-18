using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public partial class TestActual_Tests {

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
    public partial class TestActual_TReturn_Tests {

        [TestMethod]
        public void Ctor() {
            // Act
            var ret = new TestActual<DummyObject>();

            // Assert
            Assert.IsNull(ret.Return);
            Assert.IsNull(ret.Exception);
        }

        [TestMethod]
        public void Ctor_TReturn() {
            Action TestCase(int testNumber, DummyObject @return) => () => {
                var msg = "No." + testNumber;

                // Act
                var ret = new TestActual<DummyObject>(@return: @return);

                // Assert
                Assert.AreEqual(@return, ret.Return, msg);
                Assert.IsNull(ret.Exception, msg);
            };

            var obj = new DummyObject();
            foreach (var action in new[] {
                TestCase( 0, obj ),
                TestCase( 1, null),
            }) { action(); }
        }

        [TestMethod]
        public void Ctor_Exception() {
            var ex = new DummyException();

            // Act
            var ret = new TestActual<DummyObject>(exception: ex);

            // Assert
            Assert.AreEqual(default, ret.Return);
            Assert.AreEqual(ex, ret.Exception);
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
                TestCase( 0, new TestActual<DummyObject>()   , new TestActual<DummyObject>()                    , expected: true ),
                TestCase( 1, new TestActual<DummyObject>(obj), new TestActual<DummyObject>(obj)                 , expected: true ),
                TestCase( 2, new TestActual<DummyObject>(obj), new TestActual<DummyObject>(new DummyObject())   , expected: false),
                TestCase( 3, new TestActual<DummyObject>(obj), new TestActual<DummyObject>(ex)                  , expected: false),
                TestCase( 4, new TestActual<DummyObject>(ex ), new TestActual<DummyObject>(ex)                  , expected: true ),
                TestCase( 5, new TestActual<DummyObject>(ex ), new TestActual<DummyObject>(new DummyException()), expected: false),
            }) { action(); }
        }

        [TestMethod]
        public new void GetHashCode() {
            Action TestCase(int testNumber, TestActual<DummyObject> testActual) => () => {
                // GetHashCode() は環境によって戻り値が変わるので、例外が起こらない事だけ確認。
                testActual.GetHashCode();
            };

            foreach (var action in new[] {
                TestCase( 0, new TestActual<DummyObject>(@return: new DummyObject())),
                TestCase( 1, new TestActual<DummyObject>(exception: new DummyException())),
            }) { action(); }
        }

        #region Helpers

        private sealed class DummyException : Exception { }

        private sealed class DummyObject { }

        #endregion Helpers
    }
}

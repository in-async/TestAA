using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestAssertException_Tests {

        [TestMethod]
        public void Ctor() {
            var message = "abc";
            var innerException = new Exception();

            // Act
            var @return = new TestAssertException(message, innerException);

            // Assert
            Assert.AreEqual(message, @return.Message);
            Assert.AreSame(innerException, @return.InnerException);
        }
    }
}

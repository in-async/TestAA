using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class TestAssertFailedException_Tests {

        [TestMethod]
        public void Ctor_Message() {
            var message = "abc";

            // Act
            var @return = new TestAssertFailedException(message);

            // Assert
            Assert.AreEqual(message, @return.Message);
            Assert.IsNull(@return.InnerException);
        }
    }
}

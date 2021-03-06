﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class Usage {
        private static TestAssert _defaultTestAssert;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            _defaultTestAssert = TestAA.TestAssert;
            TestAA.TestAssert = new MSTestAssert();
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            TestAA.TestAssert = _defaultTestAssert;
        }

        private sealed class MSTestAssert : TestAssert {

            public override void Is<T>(T actual, T expected, string message) => Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        public void Basic() {
            TestAA.Act(() => int.Parse("123")).Assert(123);
        }

        [TestMethod]
        public void ThrowsException() {
            TestAA.Act(() => int.Parse("abc")).Assert<FormatException>();
        }

        [TestMethod]
        public void OutParameter() {
            int result = default;
            TestAA.Act(() => int.TryParse("123", out result)).Assert(true);

            // Additional Assert
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void LambdaAssert() {
            TestAA.Act(() => int.Parse("123")).Assert(
                  @return: ret => Assert.AreEqual(123, ret)
                , exception: ex => Assert.IsNull(ex)
            );
        }

        [TestMethod]
        public void TaskSynchronously() {
            TestAA.Act(() => Task.FromResult(123)).Assert(123);
            TestAA.Act(task: async () => await Task.FromResult(123)).Assert(123);

            TestAA.Act(() => new ValueTask<int>(123)).Assert(123);
        }

        [TestMethod]
        public void TaskThrowsException() {
            TestAA.Act(() => Task.FromException(new ApplicationException())).Assert<ApplicationException>();
            TestAA.Act(task: async () => await Task.FromException(new ApplicationException())).Assert<ApplicationException>();

            TestAA.Act(() => new ValueTask(Task.FromException(new ApplicationException()))).Assert<ApplicationException>();
        }

        [TestMethod]
        public void RawTask() {
            var task = Task.FromResult(123);
            TestAA.Act<Task<int>>(() => task).Assert(task);

            TestAA.Act<ValueTask<int>>(() => new ValueTask<int>(123)).Assert(new ValueTask<int>(123));
        }

        [TestMethod]
        public void ImmediateEnumerableEvaluation() {
            TestAA.Act(() => CreateEnumerable()).Assert<ApplicationException>();

            IEnumerable<int> CreateEnumerable() {
                yield return 123;
                throw new ApplicationException();
            }
        }

        [TestMethod]
        public void TestCases() {
            Action TestCase(int testNumber, string input, int expected = default, Type expectedException = null) => () => {
                var msg = "No." + testNumber;

                TestAA.Act(() => int.Parse(input)).Assert(expected, expectedException, msg);
            };

            foreach (var action in new[] {
                TestCase( 0, null , expectedException: typeof(ArgumentNullException)),
                TestCase( 1, "abc", expectedException: typeof(FormatException)),
                TestCase( 2, "123", expected: 123),
            }) { action(); }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Inasync {

    /// <summary>
    /// <see cref="TestAA"/> が使用するアサート ロジックを表します。
    /// </summary>
    public class TestAssert {
        internal static readonly TestAssert Default = new TestAssert();

        /// <summary>
        /// <see cref="TestAssert"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        protected TestAssert() {
        }

        /// <summary>
        /// 2 つの値が等しいかどうかのテストを行います。
        /// </summary>
        /// <remarks>
        /// 既定では、比較は <see cref="EqualityComparer{T}.Default"/> によって行われ、等しくないと判断された場合は <see cref="TestAssertFailedException"/> が投げられます。
        /// </remarks>
        /// <typeparam name="TReturn">比較の対象となる値の型。</typeparam>
        /// <param name="actual">実際の値。</param>
        /// <param name="expected">予期している値。</param>
        /// <param name="message"><paramref name="actual"/> と <paramref name="expected"/> が等しくない場合にテスト結果に表示されるメッセージ。</param>
        public virtual void Is<TReturn>(TReturn actual, TReturn expected, string message) {
            if (!EqualityComparer<TReturn>.Default.Equals(actual, expected)) {
                var finalMessage = "Assert failed."
                     + Environment.NewLine + "Expected: " + (expected != null ? "<" + expected + ">" : "(null)")
                     + Environment.NewLine + "Actual: " + (actual != null ? "<" + actual + ">" : "(null)")
                     + Environment.NewLine + message;

                throw new TestAssertFailedException(finalMessage);
            }
        }

        /// <summary>
        /// 生じた例外が予期した型かどうかのテストを行います。
        /// </summary>
        /// <remarks>
        /// 既定では、2 つの例外の型を <see cref="Is{TReturn}(TReturn, TReturn, string)"/> によって比較します。
        /// </remarks>
        /// <param name="actual">実際の例外。</param>
        /// <param name="expectedType">予期している例外の型。</param>
        /// <param name="message"><paramref name="actual"/> の型と <paramref name="expectedType"/> が等しくない場合にテスト結果に表示されるメッセージ。</param>
        public virtual void AssertException(Exception actual, Type expectedType, string message) {
            var finalMessage = message;
            if (actual != null) {
                finalMessage += Environment.NewLine + "Actual Exception: " + actual;
            }
            Is(actual?.GetType(), expectedType, finalMessage);
        }
    }

    public static partial class TestAA {
        private static TestAssert _testAssert;

        /// <summary>
        /// <see cref="TestAA"/> が使用するアサート ロジック。
        /// 継承した <see cref="Inasync.TestAssert"/> を設定する事で、アサート ロジックをカスタマイズできます。
        /// </summary>
        public static TestAssert TestAssert {
            get => _testAssert ?? TestAssert.Default;
            set => _testAssert = value;
        }
    }
}

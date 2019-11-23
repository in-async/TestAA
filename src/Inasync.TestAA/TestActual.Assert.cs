using System;

namespace Inasync {

    public partial struct TestActual {

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="exception">Act で生じた例外を検証するデリゲート。</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <c>null</c>.</exception>
        public void Assert(Action<Exception> exception) {
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

            exception(Exception);
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="exception">
        /// Act で生じる事が予期される例外の型。例外が生じない事が予期される場合は <c>null</c>。
        /// <see cref="TestAA.TestAssert"/> によって実際の例外と比較されます。
        /// </param>
        /// <param name="message">テスト失敗時に結果に表示されるメッセージ。</param>
        public void Assert(Type exception = null, string message = null) {
            var actual = this;
            Assert(ex => TestAA.TestAssert.AssertException(actual.Exception, exception, message));
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <typeparam name="TException">
        /// Act で生じる事が予期される例外の型。
        /// <see cref="Assert(Type, string)"/> によって実際の例外と比較されます。
        /// </typeparam>
        /// <param name="message">テスト失敗時に結果に表示されるメッセージ。</param>
        public void Assert<TException>(string message = null) where TException : Exception {
            Assert(exception: typeof(TException), message: message);
        }
    }

    public partial struct TestActual<TReturn> {

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="return">Act の戻り値を検証するデリゲート。Act で例外が生じた場合は呼ばれません。</param>
        /// <param name="exception">Act で生じた例外を検証するデリゲート。</param>
        /// <exception cref="ArgumentNullException"><paramref name="return"/> or <paramref name="exception"/> is <c>null</c>.</exception>
        public void Assert(Action<TReturn> @return, Action<Exception> exception) {
            if (@return == null) { throw new ArgumentNullException(nameof(@return)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

            // 戻り値よりも例外の検証を先にした方が、テスト失敗時に原因を特定しやすい気がする。
            exception(Exception);

            // 戻り値の検証は非例外時のみ (例外時には原理的に戻り値は存在しない (Return は必ず default になる) ので、検証する意味が無い)。
            if (Exception == null) {
                @return(Return);
            }
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="return">Act の戻り値を検証するデリゲート。Act で例外が生じた場合は呼ばれません。</param>
        /// <param name="exception">
        /// Act で生じる事が予期される例外の型。例外が生じない事が予期される場合は <c>null</c>。
        /// <see cref="TestAA.TestAssert"/> によって実際の例外と比較されます。
        /// </param>
        /// <param name="message">テスト失敗時に結果に表示されるメッセージ。</param>
        /// <exception cref="ArgumentNullException"><paramref name="return"/> is <c>null</c>.</exception>
        public void Assert(Action<TReturn> @return, Type exception = null, string message = null) {
            var actual = this;
            Assert(
                  @return
                , ex => TestAA.TestAssert.AssertException(actual.Exception, exception, message)
            );
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="return">
        /// Act の戻り値に予期される値。
        /// <see cref="TestAA.TestAssert"/> によって実際の戻り値と比較されます。
        /// Act で例外が生じた場合は比較されません。
        /// </param>
        /// <param name="exception">
        /// Act で生じる事が予期される例外の型。例外が生じない事が予期される場合は <c>null</c>。
        /// <see cref="TestAA.TestAssert"/> によって実際の例外と比較されます。
        /// </param>
        /// <param name="message">テスト失敗時に結果に表示されるメッセージ。</param>
        public void Assert(TReturn @return, Type exception = null, string message = null) {
            var actual = this;
            Assert(
                  ret => TestAA.TestAssert.Is(actual.Return, @return, message)
                , ex => TestAA.TestAssert.AssertException(actual.Exception, exception, message)
            );
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="exception">
        /// Act で生じる事が予期される例外の型。例外が生じない事が予期される場合は <c>null</c>。
        /// <see cref="TestAA.TestAssert"/> によって実際の例外と比較されます。
        /// </param>
        /// <param name="message">テスト失敗時に結果に表示されるメッセージ。</param>
        public void Assert(Type exception = null, string message = null) {
            var actual = this;
            Assert(
                  ret => { }
                , ex => TestAA.TestAssert.AssertException(actual.Exception, exception, message)
            );
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <typeparam name="TException">
        /// Act で生じる事が予期される例外の型。
        /// <see cref="Assert(Type, string)"/> によって実際の例外と比較されます。
        /// </typeparam>
        /// <param name="message">テスト失敗時に結果に表示されるメッセージ。</param>
        public void Assert<TException>(string message = null) where TException : Exception {
            Assert(exception: typeof(TException), message: message);
        }
    }
}

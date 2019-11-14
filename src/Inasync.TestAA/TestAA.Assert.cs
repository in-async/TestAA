using System;

namespace Inasync {

    public static partial class TestAA {

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="actual">Act の実行結果。</param>
        /// <param name="exception">Act で生じた例外を検証するデリゲート。</param>
        /// <param name="others">その他の Act の実行結果を検証するデリゲート。</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <c>null</c>.</exception>
        public static void Assert(this TestActual actual, Action<Exception> exception, Action others = null) {
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

            exception(actual.Exception);
            others?.Invoke();
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <typeparam name="TReturn">Act の戻り値の型。</typeparam>
        /// <param name="actual">Act の実行結果。</param>
        /// <param name="return">Act の戻り値を検証するデリゲート。</param>
        /// <param name="exception">Act で生じた例外を検証するデリゲート。</param>
        /// <param name="others">その他の Act の実行結果を検証するデリゲート。</param>
        /// <exception cref="ArgumentNullException"><paramref name="return"/> or <paramref name="exception"/> is <c>null</c>.</exception>
        public static void Assert<TReturn>(this TestActual<TReturn> actual, Action<TReturn> @return, Action<Exception> exception, Action others = null) {
            if (@return == null) { throw new ArgumentNullException(nameof(@return)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

            // 戻り値の検証は非例外時のみ (例外時には actual.Return は必ず default なので、改めて検証する必要が無い)。
            if (actual.Exception == null) {
                @return(actual.Return);
            }

            exception(actual.Exception);
            others?.Invoke();
        }
    }
}

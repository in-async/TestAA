using System;

namespace Inasync {

    public static partial class TestAA {

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <param name="actual">Act の実行結果。</param>
        /// <param name="exception">Act で生じた例外を検証するデリゲート。<c>null</c> の場合、<paramref name="actual"/> に例外が含まれていれば再スローされる。</param>
        /// <param name="others">その他の Act の実行結果を検証するデリゲート。追加の検証が必要なければ <c>null</c>。</param>
        public static void Assert(this TestActual actual, Action<Exception> exception = null, Action others = null) {
            if (exception != null) {
                exception(actual.Exception);
            }
            else if (actual.Exception != null) { throw actual.Exception; }

            others?.Invoke();
        }

        /// <summary>
        /// Act の実行結果を検証します。
        /// </summary>
        /// <typeparam name="TReturn">Act の戻り値の型。</typeparam>
        /// <param name="actual">Act の実行結果。</param>
        /// <param name="return">Act の戻り値を検証するデリゲート。</param>
        /// <param name="exception">Act で生じた例外を検証するデリゲート。<c>null</c> の場合、<paramref name="actual"/> に例外が含まれていれば再スローされる。</param>
        /// <param name="others">その他の Act の実行結果を検証するデリゲート。追加の検証が必要なければ <c>null</c>。</param>
        /// <exception cref="ArgumentNullException"><paramref name="return"/> is <c>null</c>.</exception>
        public static void Assert<TReturn>(this TestActual<TReturn> actual, Action<TReturn> @return, Action<Exception> exception = null, Action others = null) {
            if (@return == null) { throw new ArgumentNullException(nameof(@return)); }

            // 戻り値の検証は非例外時のみ (例外時には actual.Return は必ず default なので、改めて検証する必要が無い)。
            if (actual.Exception == null) {
                @return(actual.Return);
            }

            if (exception != null) {
                exception(actual.Exception);
            }
            else if (actual.Exception != null) { throw actual.Exception; }

            others?.Invoke();
        }
    }
}

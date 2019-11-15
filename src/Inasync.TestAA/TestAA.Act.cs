using System;

namespace Inasync {

    /// <summary>
    /// AAA テストパターンのうち Act 及び Assert を表します。
    /// </summary>
    public static partial class TestAA {

        /// <summary>
        /// テスト対象のデリゲートを実行します。
        /// </summary>
        /// <param name="act">テスト対象のデリゲート。</param>
        /// <returns>デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual Act(Action act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            try {
                act();
                return new TestActual(exception: null);
            }
            catch (Exception ex) {
                return new TestActual(exception: ex);
            }
        }

        /// <summary>
        /// テスト対象のデリゲートを実行します。
        /// </summary>
        /// <typeparam name="TReturn">テスト対象のデリゲートの戻り値の型。</typeparam>
        /// <param name="act">テスト対象のデリゲート。</param>
        /// <returns>デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual<TReturn> Act<TReturn>(Func<TReturn> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            try {
                var @return = act();
                return new TestActual<TReturn>(@return, exception: null);
            }
            catch (Exception ex) {
                return new TestActual<TReturn>(default, exception: ex);
            }
        }
    }
}

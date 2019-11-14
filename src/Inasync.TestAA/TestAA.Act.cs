using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// テスト対象の非同期デリゲートを実行します。
        /// </summary>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static Task<TestActual> ActAsync(Func<Task> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Internal();

            async Task<TestActual> Internal() {
                try {
                    await act().ConfigureAwait(false);
                    return new TestActual(exception: null);
                }
                catch (Exception ex) {
                    return new TestActual(exception: ex);
                }
            }
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを実行します。
        /// </summary>
        /// <typeparam name="TReturn">テスト対象の非同期デリゲートの戻り値の型。</typeparam>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static Task<TestActual<TReturn>> ActAsync<TReturn>(Func<Task<TReturn>> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Internal();

            async Task<TestActual<TReturn>> Internal() {
                try {
                    var @return = await act().ConfigureAwait(false);
                    return new TestActual<TReturn>(@return, exception: null);
                }
                catch (Exception ex) {
                    return new TestActual<TReturn>(default, exception: ex);
                }
            }
        }
    }
}

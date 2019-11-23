using System;
using System.Collections.Generic;
using System.Linq;
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
                var ret = act();
                return new TestActual<TReturn>(@return: ret);
            }
            catch (Exception ex) {
                return new TestActual<TReturn>(exception: ex);
            }
        }
    }

    public static partial class TestAA {

        /// <summary>
        /// テスト対象のデリゲートを実行し、戻り値のシーケンスを配列化します。
        /// </summary>
        /// <typeparam name="TItem">テスト対象のデリゲートが戻り値とするシーケンスの要素の型。</typeparam>
        /// <param name="act">テスト対象のデリゲート。</param>
        /// <returns>デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual<TItem[]> Act<TItem>(Func<IEnumerable<TItem>> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().ToArray());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual Act(Func<Task> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual Act(Func<ValueTask> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <typeparam name="TReturn">テスト対象の非同期デリゲートの戻り値の型。</typeparam>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual<TReturn> Act<TReturn>(Func<Task<TReturn>> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <typeparam name="TReturn">テスト対象の非同期デリゲートの戻り値の型。</typeparam>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual<TReturn> Act<TReturn>(Func<ValueTask<TReturn>> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行し、戻り値のシーケンスを配列化します。
        /// </summary>
        /// <typeparam name="TItem">テスト対象の非同期デリゲートが戻り値とするシーケンスの要素の型。</typeparam>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual<TItem[]> Act<TItem>(Func<Task<IEnumerable<TItem>>> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().GetAwaiter().GetResult().ToArray());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行し、戻り値のシーケンスを配列化します。
        /// </summary>
        /// <typeparam name="TItem">テスト対象の非同期デリゲートが戻り値とするシーケンスの要素の型。</typeparam>
        /// <param name="act">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="act"/> is <c>null</c>.</exception>
        public static TestActual<TItem[]> Act<TItem>(Func<ValueTask<IEnumerable<TItem>>> act) {
            if (act == null) { throw new ArgumentNullException(nameof(act)); }

            return Act(() => act().GetAwaiter().GetResult().ToArray());
        }
    }
}

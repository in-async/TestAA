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
        /// <param name="enumerable">テスト対象のデリゲート。</param>
        /// <returns>デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        public static TestActual<TItem[]> Act<TItem>(Func<IEnumerable<TItem>> enumerable) {
            if (enumerable == null) { throw new ArgumentNullException(nameof(enumerable)); }

            return Act(() => enumerable().ToArray());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <param name="task">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        public static TestActual Act(Func<Task> task) {
            if (task == null) { throw new ArgumentNullException(nameof(task)); }

            return Act(() => task().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <param name="valueTask">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask"/> is <c>null</c>.</exception>
        public static TestActual Act(Func<ValueTask> valueTask) {
            if (valueTask == null) { throw new ArgumentNullException(nameof(valueTask)); }

            return Act(() => valueTask().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <typeparam name="TReturn">テスト対象の非同期デリゲートの戻り値の型。</typeparam>
        /// <param name="task">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        public static TestActual<TReturn> Act<TReturn>(Func<Task<TReturn>> task) {
            if (task == null) { throw new ArgumentNullException(nameof(task)); }

            return Act(() => task().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行します。
        /// </summary>
        /// <typeparam name="TReturn">テスト対象の非同期デリゲートの戻り値の型。</typeparam>
        /// <param name="valueTask">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask"/> is <c>null</c>.</exception>
        public static TestActual<TReturn> Act<TReturn>(Func<ValueTask<TReturn>> valueTask) {
            if (valueTask == null) { throw new ArgumentNullException(nameof(valueTask)); }

            return Act(() => valueTask().GetAwaiter().GetResult());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行し、戻り値のシーケンスを配列化します。
        /// </summary>
        /// <typeparam name="TItem">テスト対象の非同期デリゲートが戻り値とするシーケンスの要素の型。</typeparam>
        /// <param name="task">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        public static TestActual<TItem[]> Act<TItem>(Func<Task<IEnumerable<TItem>>> task) {
            if (task == null) { throw new ArgumentNullException(nameof(task)); }

            return Act(() => task().GetAwaiter().GetResult().ToArray());
        }

        /// <summary>
        /// テスト対象の非同期デリゲートを同期的に実行し、戻り値のシーケンスを配列化します。
        /// </summary>
        /// <typeparam name="TItem">テスト対象の非同期デリゲートが戻り値とするシーケンスの要素の型。</typeparam>
        /// <param name="valueTask">テスト対象の非同期デリゲート。</param>
        /// <returns>非同期デリゲートの実行結果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask"/> is <c>null</c>.</exception>
        public static TestActual<TItem[]> Act<TItem>(Func<ValueTask<IEnumerable<TItem>>> valueTask) {
            if (valueTask == null) { throw new ArgumentNullException(nameof(valueTask)); }

            return Act(() => valueTask().GetAwaiter().GetResult().ToArray());
        }
    }
}

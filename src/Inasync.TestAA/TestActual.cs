using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inasync {

    /// <summary>
    /// Act の実行結果を表します。
    /// </summary>
    public readonly partial struct TestActual : IEquatable<TestActual> {

        /// <summary>
        /// Act で生じた例外を使用して、<see cref="TestActual{TReturn}"/> 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> に渡される値。Act で例外が生じていなければ <c>null</c>。</param>
        internal TestActual(Exception exception) {
            Exception = exception;
        }

        /// <summary>
        /// Act 実行中に生じた例外。
        /// </summary>
        public Exception Exception { get; }

        /// <summary>Auto Generated.</summary>
        public override bool Equals(object obj) {
            return obj is TestActual && Equals((TestActual)obj);
        }

        /// <summary>Auto Generated.</summary>
        public bool Equals(TestActual other) {
            return EqualityComparer<Exception>.Default.Equals(Exception, other.Exception);
        }

        /// <summary>Auto Generated.</summary>
        public override int GetHashCode() {
            return -311220794 + EqualityComparer<Exception>.Default.GetHashCode(Exception);
        }

        /// <summary>Auto Generated.</summary>
        public static bool operator ==(TestActual actual1, TestActual actual2) {
            return actual1.Equals(actual2);
        }

        /// <summary>Auto Generated.</summary>
        public static bool operator !=(TestActual actual1, TestActual actual2) {
            return !(actual1 == actual2);
        }
    }

    /// <summary>
    /// Act の実行結果を表します。
    /// </summary>
    /// <typeparam name="TReturn">Act の戻り値の型。</typeparam>
    public readonly partial struct TestActual<TReturn> : IEquatable<TestActual<TReturn>> {

        /// <summary>
        /// Act の戻り値を使用して、<see cref="TestActual{TReturn}"/> 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="return"><see cref="Return"/> に渡される値。</param>
        internal TestActual(TReturn @return) {
            Return = @return;
            Exception = null;
        }

        /// <summary>
        /// Act で生じた例外を使用して、<see cref="TestActual{TReturn}"/> 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> に渡される値。Not null.</param>
        internal TestActual(Exception exception) {
            Debug.Assert(exception != null);

            Return = default;
            Exception = exception;
        }

        /// <summary>
        /// Act の戻り値。
        /// Act 実行中に例外が生じた場合は <c>default</c>。
        /// </summary>
        public TReturn Return { get; }

        /// <summary>
        /// Act 実行中に生じた例外。
        /// </summary>
        public Exception Exception { get; }

        /// <summary>Auto Generated.</summary>
        public override bool Equals(object obj) {
            return obj is TestActual<TReturn> && Equals((TestActual<TReturn>)obj);
        }

        /// <summary>Auto Generated.</summary>
        public bool Equals(TestActual<TReturn> other) {
            return EqualityComparer<TReturn>.Default.Equals(Return, other.Return) &&
                   EqualityComparer<Exception>.Default.Equals(Exception, other.Exception);
        }

        /// <summary>Auto Generated.</summary>
        public override int GetHashCode() {
            var hashCode = -1738866163;
            hashCode = hashCode * -1521134295 + EqualityComparer<TReturn>.Default.GetHashCode(Return);
            hashCode = hashCode * -1521134295 + EqualityComparer<Exception>.Default.GetHashCode(Exception);
            return hashCode;
        }

        /// <summary>Auto Generated.</summary>
        public static bool operator ==(TestActual<TReturn> actual1, TestActual<TReturn> actual2) {
            return actual1.Equals(actual2);
        }

        /// <summary>Auto Generated.</summary>
        public static bool operator !=(TestActual<TReturn> actual1, TestActual<TReturn> actual2) {
            return !(actual1 == actual2);
        }
    }
}

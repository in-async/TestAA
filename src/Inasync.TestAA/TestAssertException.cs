using System;

#if !NETSTANDARD1_0

using System.Runtime.Serialization;

#endif

namespace Inasync {
    /// <summary>
    /// Assert 時に検証されなかった <see cref="TestActual.Exception"/> を内包する例外を表します。
    /// </summary>
#if !NETSTANDARD1_0

    [Serializable]
#endif
    public class TestAssertException : Exception {
        private const string _defaultMessage = "TestActual に例外が含まれていますが、検証されませんでした。";

        /// <summary>
        /// 指定したエラー メッセージおよびこの例外の原因となった内部例外への参照を使用して、<see cref="TestAssertException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        /// <param name="innerException">現在の例外の原因である例外。内部例外が指定されていない場合は <c>null</c> 参照。</param>
        public TestAssertException(string message, Exception innerException) : base(message ?? _defaultMessage, innerException) {
        }

#if !NETSTANDARD1_0

        /// <summary>
        /// シリアル化したデータを使用して、<see cref="TestAssertException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">スローされている例外に関するシリアル化済みオブジェクト データを保持している <see cref="SerializationInfo"/>。</param>
        /// <param name="context">転送元または転送先についてのコンテキスト情報を含む <see cref="StreamingContext"/>。</param>
        protected TestAssertException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

#endif
    }
}

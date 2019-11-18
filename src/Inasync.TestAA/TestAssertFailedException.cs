using System;

namespace Inasync {

    /// <summary>
    /// <see cref="TestAssert"/> に失敗した時に送出される例外を表します。
    /// </summary>
    public class TestAssertFailedException : Exception {

        /// <summary>
        /// 指定したエラー メッセージを使用して、<see cref="TestAssertFailedException"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">エラーを説明するメッセージ。</param>
        public TestAssertFailedException(string message) : base(message) {
        }
    }
}

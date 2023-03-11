using System;

namespace MornLib.Inputs
{
    /// <summary>
    ///     MornInputSystemUtilから入力を取得するInterface
    /// </summary>
    /// <typeparam name="TActionEnum">入力を判別するenum</typeparam>
    public interface IMornInputSystemUtilUser<in TActionEnum> where TActionEnum : Enum
    {
        /// <summary>
        ///     キャッシュしたAxis入力を返す
        /// </summary>
        /// <param name="negativeActionEnum">負値の判定をするActionEnum</param>
        /// <param name="positiveActionEnum">正値の判定をするActionEnum</param>
        /// <returns>{-1,0,1}のいずれかを返す</returns>
        float GetAxisRaw(TActionEnum negativeActionEnum, TActionEnum positiveActionEnum);

        /// <summary>
        ///     キャッシュしたButton入力を返す
        /// </summary>
        /// <param name="actionEnum">取得するキャッシュのActionEnum</param>
        /// <param name="disposeCacheIfUseCache">キャッシュ利用時、キャッシュを破棄するか</param>
        /// <returns>Button入力の有無</returns>
        bool GetCachedButton(TActionEnum actionEnum, bool disposeCacheIfUseCache = true);
    }
}

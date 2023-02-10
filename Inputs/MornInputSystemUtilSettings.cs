using System;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace MornLib.Inputs
{
    /// <summary>
    ///     MornInputSystemUtilの設定データ
    /// </summary>
    [Serializable]
    public readonly struct MornInputSystemUtilSettings
    {
        /// <summary>
        ///     利用するInputActionMap
        /// </summary>
        public readonly InputActionMap InputActionMap;

        /// <summary>
        ///     入力の有効時間
        /// </summary>
        public readonly float KeepCacheTime;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="inputActionMap">利用するInputActionMap</param>
        /// <param name="keepCacheTime">入力の有効時間</param>
        public MornInputSystemUtilSettings(InputActionMap inputActionMap, float keepCacheTime)
        {
            Assert.IsNotNull(inputActionMap);
            Assert.IsTrue(keepCacheTime >= 0);
            KeepCacheTime = keepCacheTime;
            InputActionMap = inputActionMap;
        }
    }
}

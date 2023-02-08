using System;
using System.Collections.Generic;
using MornLib.Cores;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace MornLib.Inputs
{
    /// <summary>
    ///     InputSystemの拡張。入力を指定時間キャッシュできる。
    /// </summary>
    /// <typeparam name="TActionEnum">入力を判別するenum</typeparam>
    public sealed class MornInputSystemUtil<TActionEnum> where TActionEnum : Enum
    {
        /// <summary>
        ///     入力判定に用いるInputActionMap
        /// </summary>
        private readonly InputActionMap _actionMap;

        /// <summary>
        ///     入力をキャッシュする時間
        /// </summary>
        private readonly float _keepCacheTime;

        /// <summary>
        ///     Button入力の残り有効時間
        /// </summary>
        private readonly Dictionary<TActionEnum, float> _buttonValidTimeDictionary = new();

        /// <summary>
        ///     Axis入力の残り有効時間
        /// </summary>
        private readonly Dictionary<TActionEnum, bool> _axisActiveDictionary = new();

        /// <summary>
        ///     Button入力List
        /// </summary>
        private readonly List<TActionEnum> _buttonList = new();

        /// <summary>
        ///     Axis入力List
        /// </summary>
        private readonly List<TActionEnum> _axisList = new();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="actionMap">利用するInputActionMap</param>
        /// <param name="keepCacheTime">入力の有効時間</param>
        public MornInputSystemUtil(InputActionMap actionMap, float keepCacheTime)
        {
            Assert.IsNotNull(actionMap);
            Assert.IsTrue(keepCacheTime >= 0);
            _actionMap = actionMap;
            _keepCacheTime = keepCacheTime;
        }

        /// <summary>
        ///     キャッシュするActionをEnumで指定
        /// </summary>
        /// <param name="actionEnum">登録するEnum</param>
        /// <param name="isButton">ButtonかAxisか</param>
        public void RegisterAction(TActionEnum actionEnum, bool isButton)
        {
            Assert.IsFalse(_buttonList.Contains(actionEnum));
            Assert.IsFalse(_axisList.Contains(actionEnum));
            Assert.IsFalse(_buttonValidTimeDictionary.ContainsKey(actionEnum));
            Assert.IsFalse(_axisActiveDictionary.ContainsKey(actionEnum));
            if (isButton)
            {
                _buttonList.Add(actionEnum);
                _buttonValidTimeDictionary.Add(actionEnum, 0);
            }
            else
            {
                _axisList.Add(actionEnum);
                _axisActiveDictionary.Add(actionEnum, false);
            }
        }

        /// <summary>
        ///     キャッシュしたButton入力を返す
        /// </summary>
        /// <param name="actionEnum">取得するキャッシュのActionEnum</param>
        /// <param name="disposeCacheIfUseCache">キャッシュ利用時、キャッシュを破棄するか</param>
        /// <returns>Button入力の有無</returns>
        public bool GetCachedButton(TActionEnum actionEnum, bool disposeCacheIfUseCache = true)
        {
            Assert.IsTrue(_buttonValidTimeDictionary.ContainsKey(actionEnum));
            if (_buttonValidTimeDictionary[actionEnum] > 0)
            {
                if (disposeCacheIfUseCache)
                {
                    _buttonValidTimeDictionary[actionEnum] = 0;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     キャッシュしたAxis入力を返す
        /// </summary>
        /// <param name="negativeActionEnum">負値の判定をするActionEnum</param>
        /// <param name="positiveActionEnum">正値の判定をするActionEnum</param>
        /// <returns>{-1,0,1}のいずれかを返す</returns>
        public float GetAxisRaw(TActionEnum negativeActionEnum, TActionEnum positiveActionEnum)
        {
            Assert.IsTrue(_axisActiveDictionary.ContainsKey(negativeActionEnum));
            Assert.IsTrue(_axisActiveDictionary.ContainsKey(positiveActionEnum));
            var hor = 0;
            if (_axisActiveDictionary[negativeActionEnum])
            {
                hor--;
            }

            if (_axisActiveDictionary[positiveActionEnum])
            {
                hor++;
            }

            return hor;
        }

        /// <summary>
        ///     入力を更新する
        /// </summary>
        /// <param name="deltaTime">キャッシュ更新に用いるdeltaTime</param>
        public void UpdateInput(float deltaTime)
        {
            Assert.IsTrue(deltaTime >= 0);
            foreach (var buttonEnum in _buttonList)
            {
                Assert.IsTrue(_buttonValidTimeDictionary.ContainsKey(buttonEnum));
                var name = MornEnum<TActionEnum>.CachedToString(buttonEnum);
                if (_actionMap[name].WasPressedThisFrame())
                {
                    _buttonValidTimeDictionary[buttonEnum] = _keepCacheTime;
                }
                else
                {
                    _buttonValidTimeDictionary[buttonEnum] -= deltaTime;
                }
            }

            foreach (var axisEnum in _axisList)
            {
                Assert.IsTrue(_axisActiveDictionary.ContainsKey(axisEnum));
                var name = MornEnum<TActionEnum>.CachedToString(axisEnum);
                _axisActiveDictionary[axisEnum] = _actionMap[name].IsPressed();
            }
        }
    }
}

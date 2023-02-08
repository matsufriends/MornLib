using System;
using System.Collections.Generic;
using MornLib.Cores;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace MornLib.Inputs
{
    /// <summary>
    ///     InputSystemの拡張。入力を指定時間キャッシュする。
    /// </summary>
    /// <typeparam name="TActionEnum">入力を判別するenum</typeparam>
    public sealed class MornCachedInputSystemUtil<TActionEnum> where TActionEnum : Enum
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
        ///     入力の残り有効時間
        /// </summary>
        private readonly Dictionary<TActionEnum, float> _inputValidTimeDictionary = new();

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
        public MornCachedInputSystemUtil(InputActionMap actionMap, float keepCacheTime)
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
        public void RegisterCacheAction(TActionEnum actionEnum, bool isButton)
        {
            Assert.IsFalse(_buttonList.Contains(actionEnum));
            Assert.IsFalse(_axisList.Contains(actionEnum));
            Assert.IsFalse(_inputValidTimeDictionary.ContainsKey(actionEnum));
            if (isButton)
            {
                _buttonList.Add(actionEnum);
            }
            else
            {
                _axisList.Add(actionEnum);
            }

            _inputValidTimeDictionary.Add(actionEnum, 0);
        }

        /// <summary>
        ///     キャッシュしたAxis入力を返す
        /// </summary>
        /// <param name="negativeActionEnum">負値の判定をするActionEnum</param>
        /// <param name="positiveActionEnum">正値の判定をするActionEnum</param>
        /// <returns>{-1,0,1}のいずれかを返す</returns>
        public float GetCachedAxisRaw(TActionEnum negativeActionEnum, TActionEnum positiveActionEnum)
        {
            Assert.IsTrue(_inputValidTimeDictionary.ContainsKey(negativeActionEnum));
            Assert.IsTrue(_inputValidTimeDictionary.ContainsKey(positiveActionEnum));
            var hor = 0;
            if (_inputValidTimeDictionary[negativeActionEnum] > 0)
            {
                hor--;
            }

            if (_inputValidTimeDictionary[positiveActionEnum] > 0)
            {
                hor++;
            }

            return hor;
        }

        /// <summary>
        ///     キャッシュしたButton入力を返す
        /// </summary>
        /// <param name="actionEnum">取得するキャッシュのActionEnum</param>
        /// <param name="disposeCacheIfUseCache">キャッシュ利用時、キャッシュを破棄するか</param>
        /// <returns>Button入力の有無</returns>
        public bool GetCachedButton(TActionEnum actionEnum, bool disposeCacheIfUseCache = true)
        {
            Assert.IsTrue(_inputValidTimeDictionary.ContainsKey(actionEnum));
            if (_inputValidTimeDictionary[actionEnum] > 0)
            {
                if (disposeCacheIfUseCache)
                {
                    _inputValidTimeDictionary[actionEnum] = 0;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     キャッシュを更新する
        /// </summary>
        /// <param name="deltaTime">キャッシュ更新に用いるdeltaTime</param>
        public void UpdateCache(float deltaTime)
        {
            Assert.IsTrue(deltaTime >= 0);
            foreach (var buttonEnum in _buttonList)
            {
                Assert.IsTrue(_inputValidTimeDictionary.ContainsKey(buttonEnum));
                var name = MornEnum<TActionEnum>.CachedToString(buttonEnum);
                if (_actionMap[name].WasPressedThisFrame())
                {
                    _inputValidTimeDictionary[buttonEnum] = _keepCacheTime;
                }
                else
                {
                    _inputValidTimeDictionary[buttonEnum] -= deltaTime;
                }
            }

            foreach (var axisEnum in _axisList)
            {
                Assert.IsTrue(_inputValidTimeDictionary.ContainsKey(axisEnum));
                var name = MornEnum<TActionEnum>.CachedToString(axisEnum);
                _inputValidTimeDictionary[axisEnum] = _actionMap[name].IsPressed() ? 1 : 0;
            }
        }
    }
}

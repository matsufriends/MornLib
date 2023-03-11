using System;
using System.Collections.Generic;
using MornEnum.Runtime;
using UnityEngine.Assertions;

namespace MornLib.Inputs
{
    /// <summary>InputSystemの拡張。入力を指定時間キャッシュできる。</summary>
    /// <typeparam name="TActionEnum">入力を判別するenum</typeparam>
    public sealed class MornInputSystemUtil<TActionEnum> : IMornInputSystemUtilUser<TActionEnum> where TActionEnum : Enum
    {
        /// <summary>初期化時に受け取る設定データ</summary>
        private readonly MornInputSystemUtilSettings _settings;

        /// <summary>Button入力の残り有効時間</summary>
        private readonly Dictionary<TActionEnum, float> _buttonValidTimeDictionary = new();

        /// <summary>Axis入力の残り有効時間</summary>
        private readonly Dictionary<TActionEnum, bool> _axisActiveDictionary = new();

        /// <summary>Button入力List</summary>
        private readonly List<TActionEnum> _buttonList = new();

        /// <summary>Axis入力List</summary>
        private readonly List<TActionEnum> _axisList = new();

        /// <summary>コンストラクタ</summary>
        /// <param name="settings">設定データ</param>
        public MornInputSystemUtil(MornInputSystemUtilSettings settings)
        {
            _settings = settings;
        }

        /// <summary>キャッシュするActionをEnumで指定</summary>
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

        bool IMornInputSystemUtilUser<TActionEnum>.GetCachedButton(TActionEnum actionEnum, bool disposeCacheIfUseCache = true)
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

        float IMornInputSystemUtilUser<TActionEnum>.GetAxisRaw(TActionEnum negativeActionEnum, TActionEnum positiveActionEnum)
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

        /// <summary>入力を更新する</summary>
        /// <param name="deltaTime">キャッシュ更新に用いるdeltaTime</param>
        public void UpdateInput(float deltaTime)
        {
            Assert.IsTrue(deltaTime >= 0);
            foreach (var buttonEnum in _buttonList)
            {
                Assert.IsTrue(_buttonValidTimeDictionary.ContainsKey(buttonEnum));
                var name = MornEnumUtil<TActionEnum>.CachedToString(buttonEnum);
                if (_settings.InputActionMap[name].WasPressedThisFrame())
                {
                    _buttonValidTimeDictionary[buttonEnum] = _settings.KeepCacheTime;
                }
                else
                {
                    _buttonValidTimeDictionary[buttonEnum] -= deltaTime;
                }
            }

            foreach (var axisEnum in _axisList)
            {
                Assert.IsTrue(_axisActiveDictionary.ContainsKey(axisEnum));
                var name = MornEnumUtil<TActionEnum>.CachedToString(axisEnum);
                _axisActiveDictionary[axisEnum] = _settings.InputActionMap[name].IsPressed();
            }
        }
    }
}

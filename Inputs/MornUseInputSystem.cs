using System;
using System.Collections.Generic;
using MornLib.Cores;
using UnityEngine.InputSystem;

namespace MornLib.Inputs
{
    public sealed class MornUseInputSystem<TActionEnum> where TActionEnum : Enum
    {
        private readonly InputActionMap _actionMap;
        private readonly float _keepTimes;
        private readonly Dictionary<TActionEnum, float> _actFlagDictionary = new();
        private readonly Dictionary<TActionEnum, string> _actToStringDictionary = new();
        private readonly List<TActionEnum> _buttonList = new();
        private readonly List<TActionEnum> _axisList = new();

        public MornUseInputSystem(InputActionMap actionMap, float keepTime)
        {
            _actionMap = actionMap;
            _keepTimes = keepTime;
            foreach (var act in MornEnum<TActionEnum>.Values)
            {
                _actFlagDictionary.Add(act, 0);
                _actToStringDictionary.Add(act, act.ToString());
            }
        }

        public void Register(TActionEnum action, bool isButton)
        {
            if (isButton)
            {
                _buttonList.Add(action);
            }
            else
            {
                _axisList.Add(action);
            }
        }

        public float GetAxis(TActionEnum negative, TActionEnum positive)
        {
            var hor = 0;
            if (_actFlagDictionary[negative] > 0)
            {
                hor--;
            }

            if (_actFlagDictionary[positive] > 0)
            {
                hor++;
            }

            return hor;
        }

        public bool TryRemoveFlag(TActionEnum act)
        {
            if (_actFlagDictionary[act] > 0)
            {
                _actFlagDictionary[act] = 0;
                return true;
            }

            return false;
        }

        public void BeforeUpdate()
        {
            foreach (var button in _buttonList)
            {
                var name = _actToStringDictionary[button];
                if (_actionMap[name].WasPressedThisFrame())
                {
                    _actFlagDictionary[button] = _keepTimes;
                }
            }

            foreach (var axis in _axisList)
            {
                var name = _actToStringDictionary[axis];
                _actFlagDictionary[axis] = _actionMap[name].IsPressed() ? 1 : 0;
            }
        }

        public void AfterUpdate(float deltaTime)
        {
            foreach (var button in _buttonList)
            {
                _actFlagDictionary[button] -= deltaTime;
            }
        }
    }
}

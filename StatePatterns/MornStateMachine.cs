using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornLib.StatePatterns
{
    public sealed class MornStateMachine<TEnum, TArg> where TEnum : Enum
    {
        private readonly Dictionary<TEnum, Action<TEnum>> _enterDictionary = new();
        private readonly Dictionary<TEnum, Func<TArg, TEnum>> _handleDictionary = new();
        private float _startTime = -1;
        private readonly bool _useUnScaledTime;
        private readonly TEnum _noneEnum;
        public TEnum CurState { get; private set; }
        public bool IsFirst => Frame == 0;
        public int Frame { get; private set; }
        public float PlayingTime => (_useUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;

        public MornStateMachine(TEnum noneEnum, bool useUnscaledTime = false)
        {
            _noneEnum = noneEnum;
            _useUnScaledTime = useUnscaledTime;
            Frame = 0;
        }

        public void InitState(TEnum state)
        {
            CurState = state;
        }

        public void Handle(TArg arg)
        {
            if (_startTime < 0)
            {
                _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            }

            var newState = _handleDictionary[CurState](arg);
            Frame++;
            if (EqualityComparer<TEnum>.Default.Equals(newState, _noneEnum) == false)
            {
                ChangeState(newState);
            }
        }

        public void RegisterState(TEnum type, Action<TEnum> enter, Func<TArg, TEnum> task)
        {
            _enterDictionary.Add(type, enter);
            _handleDictionary.Add(type, task);
        }

        public void ChangeState(TEnum type)
        {
            var old = CurState;
            CurState = type;
            Frame = 0;
            _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            _enterDictionary[CurState](old);
        }

        public bool IsState(TEnum type)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type);
        }
    }
}

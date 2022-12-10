using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornLib.StatePatterns
{
    public sealed class MornStateMachine<TEnum, TArg> where TEnum : Enum
    {
        private readonly Dictionary<TEnum, Func<TArg, TEnum>> _taskDictionary = new();
        private float _startTime = -1;
        private readonly bool _useUnScaledTime;
        public TEnum CurState { get; private set; }
        public bool IsFirst => Frame == 0;
        public int Frame { get; private set; }
        public float PlayingTime => (_useUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;

        public MornStateMachine(TEnum initType, bool useUnscaledTime = false)
        {
            _useUnScaledTime = useUnscaledTime;
            CurState = initType;
            Frame = 0;
        }

        public void Handle(TArg arg)
        {
            if (_startTime < 0)
            {
                _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            }

            var newState = _taskDictionary[CurState](arg);
            Frame++;
            if (IsState(newState) == false)
            {
                ChangeState(newState);
            }

            Frame++;
        }

        public void RegisterState(TEnum type, Func<TArg, TEnum> task)
        {
            _taskDictionary.Add(type, task);
        }

        private void ChangeState(TEnum type)
        {
            CurState = type;
            Frame = 0;
            _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
        }

        public bool IsState(TEnum type)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type);
        }
    }
}

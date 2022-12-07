using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornLib.StatePattern
{
    public sealed class MornStateMachine<TEnum, TArg> where TEnum : Enum
    {
        private readonly Dictionary<TEnum, Action<TArg>> _taskDictionary = new();
        private float _startTime = -1;
        private bool _isStateChanged;
        private readonly bool _useUnScaledTime;
        public TEnum CurState { get; private set; }
        public bool IsFirst { get; private set; }
        public float PlayingTime => (_useUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;

        public MornStateMachine(TEnum initType, bool useUnscaledTime = false)
        {
            _useUnScaledTime = useUnscaledTime;
            CurState = initType;
            IsFirst = true;
            _isStateChanged = false;
        }

        public void Handle(TArg arg)
        {
            if (_startTime < 0)
            {
                _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            }

            _taskDictionary[CurState](arg);
            if (_isStateChanged == false)
            {
                IsFirst = false;
            }

            _isStateChanged = false;
        }

        public void RegisterState(TEnum type, Action<TArg> task)
        {
            _taskDictionary.Add(type, task);
        }

        public void ChangeState(TEnum type)
        {
            CurState = type;
            _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            IsFirst = true;
            _isStateChanged = true;
        }

        public bool IsState(TEnum type)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type);
        }
    }
}

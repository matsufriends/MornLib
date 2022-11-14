using System;
using System.Collections.Generic;
using UnityEngine;
namespace MornLib.Cores {
    public sealed class MornSm<TEnum,TArg> where TEnum : Enum {
        private readonly Dictionary<TEnum,Action<TArg>> _taskDictionary = new();
        private TEnum _curState;
        private float _startTime = -1;
        private bool _isStateChanged;
        private readonly bool _isUnScaledTime;
        public bool IsFirst { get; private set; }
        public float PlayingTime => (_isUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;
        public MornSm(TEnum initType,bool isUnscaledTime) {
            _isUnScaledTime = isUnscaledTime;
            _curState       = initType;
            IsFirst         = true;
            _isStateChanged = false;
        }
        public void Handle(TArg arg) {
            if(_startTime < 0) _startTime = _isUnScaledTime ? Time.unscaledTime : Time.time;
            _taskDictionary[_curState].Invoke(arg);
            if(_isStateChanged == false) IsFirst = false;
            _isStateChanged = false;
        }
        public void RegisterState(TEnum type,Action<TArg> task) => _taskDictionary.Add(type,task);
        public void ChangeState(TEnum type) {
            _curState       = type;
            _startTime      = _isUnScaledTime ? Time.unscaledTime : Time.time;
            IsFirst         = true;
            _isStateChanged = true;
        }
        public bool IsState(TEnum type) => EqualityComparer<TEnum>.Default.Equals(_curState,type);
    }
}
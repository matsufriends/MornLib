using System;
using System.Collections.Generic;
using UnityEngine;
namespace MornLib.Cores {
    public sealed class MornSm<TEnum,TArg> where TEnum : Enum {
        private readonly Dictionary<TEnum,Action<TArg>> _taskDictionary = new();
        private TEnum _curState;
        private float _startTime;
        private readonly bool _isUnScaledTime;
        public bool IsFirst { get; private set; }
        public float PlayingTime => (_isUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;
        public MornSm(TEnum initType,bool isUnscaledTime) {
            _isUnScaledTime = isUnscaledTime;
            ChangeState(initType);
        }
        public void Handle(TArg arg) {
            var cacheState = _curState;
            _taskDictionary[_curState].Invoke(arg);
            if(EqualityComparer<TEnum>.Default.Equals(cacheState,_curState)) IsFirst = false;
        }
        public void RegisterState(TEnum type,Action<TArg> task) {
            _taskDictionary.Add(type,task);
        }
        public void ChangeState(TEnum type) {
            _curState = type;
            try {
                _startTime = _isUnScaledTime ? Time.unscaledTime : Time.time;
            } catch {
                _startTime = 0;
            }
            IsFirst = true;
        }
        public bool IsState(TEnum type) {
            return EqualityComparer<TEnum>.Default.Equals(_curState,type);
        }
    }
}
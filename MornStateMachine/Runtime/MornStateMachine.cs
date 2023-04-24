using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MornStateMachine
{
    public sealed class MornStateMachine<TEnum, TArg> where TEnum : Enum
    {
        private readonly Subject<TEnum> _onEnterSubject = new();
        private readonly Subject<TEnum> _onExitSubject = new();
        private readonly Dictionary<TEnum, Func<TArg, TEnum>> _updateDictionary = new();
        private readonly bool _useUnScaledTime;
        private readonly TEnum _noneEnum;
        public TEnum CurState { get; private set; }
        public bool IsFirst => Frame == 0;
        public int Frame { get; private set; }
        public float PlayingTime { get; private set; }
        public IObservable<TEnum> OnEnter => _onEnterSubject;
        public IObservable<TEnum> OnExit => _onExitSubject;

        public MornStateMachine(TEnum noneEnum, bool useUnscaledTime = false)
        {
            _noneEnum = noneEnum;
            CurState = _noneEnum;
            _useUnScaledTime = useUnscaledTime;
            Frame = 0;
        }

        public void Handle(TArg arg)
        {
            var newState = _updateDictionary[CurState](arg);
            Frame++;
            PlayingTime += _useUnScaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (EqualityComparer<TEnum>.Default.Equals(newState, _noneEnum) == false)
            {
                ChangeState(newState);
            }
        }

        public void RegisterState(TEnum type, Func<TArg, TEnum> task)
        {
            _updateDictionary.Add(type, task);
        }

        public void ChangeState(TEnum type)
        {
            _onExitSubject.OnNext(CurState);
            CurState = type;
            Frame = 0;
            PlayingTime = 0;
            _onEnterSubject.OnNext(CurState);
        }

        public bool IsState(TEnum type)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type);
        }
    }
}

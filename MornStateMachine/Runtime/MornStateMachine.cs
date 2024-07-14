using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MornStateMachine
{
    public sealed class MornStateMachine<TEnum, TArg> : IMornStateMachineFrame where TEnum : Enum
    {
        private readonly int _fps;
        private readonly TEnum _noneEnum;
        private readonly Subject<TEnum> _onEnterSubject = new();
        private readonly Subject<TEnum> _onExitSubject = new();
        private readonly Dictionary<TEnum, Func<TArg, TEnum>> _updateDictionary = new();
        private readonly bool _useUnScaledTime;

        public MornStateMachine(TEnum noneEnum, bool useUnscaledTime = false, int fps = 60)
        {
            _noneEnum = noneEnum;
            CurState = _noneEnum;
            _useUnScaledTime = useUnscaledTime;
            _fps = fps;
        }

        public int ChangeSameStateCount { get; private set; }
        public TEnum CurState { get; private set; }
        public IObservable<TEnum> OnEnter => _onEnterSubject;
        public IObservable<TEnum> OnExit => _onExitSubject;
        public float PrevFrame { get; private set; }
        public float Frame { get; private set; }

        public bool IsFrame(int frame)
        {
            return PrevFrame < frame && frame <= Frame;
        }

        public void Handle(TArg arg)
        {
            PrevFrame = Frame;
            Frame += _fps * (_useUnScaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
            var newState = _updateDictionary[CurState](arg);
            if (EqualityComparer<TEnum>.Default.Equals(newState, _noneEnum) == false) ChangeState(newState);
        }

        public void RegisterState(TEnum type, Func<TArg, TEnum> task)
        {
            _updateDictionary.Add(type, task);
        }

        public void ChangeState(TEnum type)
        {
            _onExitSubject.OnNext(CurState);
            if (IsState(type))
                ChangeSameStateCount++;
            else
                ChangeSameStateCount = 0;

            CurState = type;
            PrevFrame = 0;
            Frame = 0;
            _onEnterSubject.OnNext(CurState);
        }

        public bool IsState(TEnum type)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type);
        }

        public bool IsState(TEnum type1, TEnum type2)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type1) ||
                   EqualityComparer<TEnum>.Default.Equals(CurState, type2);
        }

        public bool IsState(TEnum type1, TEnum type2, TEnum type3)
        {
            return EqualityComparer<TEnum>.Default.Equals(CurState, type1) ||
                   EqualityComparer<TEnum>.Default.Equals(CurState, type2) ||
                   EqualityComparer<TEnum>.Default.Equals(CurState, type3);
        }

        public bool IsState(params TEnum[] types)
        {
            foreach (var type in types)
                if (EqualityComparer<TEnum>.Default.Equals(CurState, type))
                    return true;

            return false;
        }
    }
}
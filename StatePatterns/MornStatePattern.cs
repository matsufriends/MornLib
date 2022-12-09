using UnityEngine;

namespace MornLib.StatePatterns
{
    public sealed class MornStatePattern<TInterface, TArg> where TInterface : class, IMornStatePattern<TArg>
    {
        private TInterface _state;
        private float _startTime = -1;
        private readonly bool _useUnScaledTime;
        public bool IsFirst { get; private set; }
        public float PlayingTime => (_useUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;

        public MornStatePattern(TInterface initState, bool useUnscaledTime = false)
        {
            _useUnScaledTime = useUnscaledTime;
            _state = initState;
            IsFirst = true;
        }

        public void Handle(TArg arg)
        {
            if (_state == null)
            {
                return;
            }

            if (_startTime < 0)
            {
                _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            }

            var newState = _state.Execute(arg);
            IsFirst = false;
            if (newState != null && IsState((TInterface)newState) == false)
            {
                _state = (TInterface)newState;
                IsFirst = true;
                _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            }
        }

        public void ChangeState(TInterface state)
        {
            _state = state;
            _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
            IsFirst = true;
        }

        public bool IsState(TInterface state)
        {
            return ReferenceEquals(_state, state);
        }
    }
}

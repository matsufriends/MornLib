using UnityEngine;

namespace MornLib.StatePatterns
{
    public sealed class MornStatePattern<TInterface, TArg> where TInterface : class, IMornStatePattern<TArg>
    {
        private readonly bool _useUnScaledTime;
        private TInterface _state;
        private float _startTime = -1;
        public bool IsFirst => Frame == 0;
        public int Frame { get; private set; }
        public float PlayingTime => (_useUnScaledTime ? Time.unscaledTime : Time.time) - _startTime;

        public MornStatePattern(TInterface initState, bool useUnscaledTime = false)
        {
            _useUnScaledTime = useUnscaledTime;
            _state = initState;
            Frame = 0;
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
            Frame++;
            if (newState != null && IsState((TInterface)newState) == false)
            {
                ChangeState((TInterface)newState);
            }
        }

        public void ChangeState(TInterface state)
        {
            _state = state;
            Frame = 0;
            _startTime = _useUnScaledTime ? Time.unscaledTime : Time.time;
        }

        public bool IsState(TInterface state)
        {
            return ReferenceEquals(_state, state);
        }
    }
}

using System;
using UnityEngine;

namespace MornBeat
{
    [Serializable]
    public struct MornBeatAction<TEnum> where TEnum : Enum
    {
        [SerializeField] private int _measure;
        [SerializeField] private int _tick;
        [SerializeField] private TEnum _beatActionType;
        public int Measure => _measure;
        public int Tick => _tick;
        public TEnum BeatActionType => _beatActionType;

        public MornBeatAction(int measure, int tick, TEnum beatActionType)
        {
            _measure = measure;
            _tick = tick;
            _beatActionType = beatActionType;
        }
    }
}
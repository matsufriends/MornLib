﻿using System;
using MornLib.Cores;
using MornLib.Singletons;
using UniRx;
using UnityEngine;

namespace MornLib.Beats
{
    public abstract class MornBeatManagerMonoBase<TBeatEnum> : MornSingletonMono<MornBeatManagerMonoBase<TBeatEnum>>, IMornBeatObservable
        where TBeatEnum : Enum
    {
        [Header("MakeBeat"), SerializeField] private MornSerializableDictionaryProvider<TBeatEnum, MornBeatMemoSo> _beatDictionary;
        private int _nextBeatIndex;
        private MornBeatMemoSo _memo;
        private float _lastBgmTime;
        private bool _waitLoop;
        private readonly Subject<int> _beatSubject = new();
        private readonly Subject<Unit> _endBeatSubject = new();
        public IObservable<int> OnBeat => _beatSubject;
        public IObservable<Unit> OnEndBeat => _endBeatSubject;

        //次の小節までの時間
        public float LeftTime { get; private set; }
        public abstract void MyUpdate();

        protected void MyUpdateImpl(float time)
        {
            if (_memo == null)
            {
                return;
            }

            if (_waitLoop)
            {
                if (_lastBgmTime <= time)
                {
                    return;
                }

                _waitLoop = false;
            }

            _lastBgmTime = time;
            LeftTime = _memo.GetBeatTiming(Mathf.FloorToInt(_nextBeatIndex / 8f) * 8 + 7) - _lastBgmTime;
            if (_lastBgmTime < _memo.GetBeatTiming(_nextBeatIndex))
            {
                return;
            }

            _beatSubject.OnNext(_nextBeatIndex);
            _waitLoop = _memo.GetBeatTiming(_nextBeatIndex) > _memo.GetBeatTiming(_nextBeatIndex + 1);
            _nextBeatIndex++;
            if (_nextBeatIndex == _memo.Timings)
            {
                _nextBeatIndex = 0;
                _waitLoop = false;
                _endBeatSubject.OnNext(Unit.Default);
            }
        }

        public abstract void BeatStart(TBeatEnum beatType);

        protected void BeatStartImpl(TBeatEnum beatType)
        {
            _nextBeatIndex = 0;
            _memo = _beatDictionary[beatType];
            _waitLoop = false;
        }
    }
}

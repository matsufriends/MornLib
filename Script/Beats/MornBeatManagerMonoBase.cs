using System;
using MornLib.Cores;
using MornSingleton;
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
        private readonly Subject<BeatTimingInfo> _beatSubject = new();
        private readonly Subject<Unit> _endBeatSubject = new();
        public IObservable<BeatTimingInfo> OnBeat => _beatSubject;
        public IObservable<Unit> OnEndBeat => _endBeatSubject;

        //次の小節までの時間
        public float LeftMeasureTime { get; private set; }
        protected abstract float MusicPlayingTime { get; }

        public void MyUpdate()
        {
            MyUpdateImpl(MusicPlayingTime);
        }

        private void MyUpdateImpl(float time)
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
            LeftMeasureTime = _memo.GetBeatTiming(Mathf.FloorToInt(_nextBeatIndex / 8f) * 8 + 7) - _lastBgmTime;
            if (_lastBgmTime < _memo.GetBeatTiming(_nextBeatIndex))
            {
                return;
            }

            _beatSubject.OnNext(new BeatTimingInfo(_nextBeatIndex, _memo.BeatCount));
            _waitLoop = _memo.GetBeatTiming(_nextBeatIndex) > _memo.GetBeatTiming(_nextBeatIndex + 1);
            _nextBeatIndex++;
            if (_nextBeatIndex == _memo.Timings)
            {
                _nextBeatIndex = 0;
                _waitLoop = false;
                _endBeatSubject.OnNext(Unit.Default);
            }
        }

        public void BeatStart(TBeatEnum beatType)
        {
            _nextBeatIndex = 0;
            _memo = _beatDictionary[beatType];
            _waitLoop = false;
            BeatStartImpl(beatType, _memo.clip);
        }

        protected abstract void BeatStartImpl(TBeatEnum beatType, AudioClip clip);
    }
}

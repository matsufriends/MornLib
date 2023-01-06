using System;
using MornLib.Cores;
using UniRx;
using UnityEngine;

namespace MornLib.Beats
{
    public abstract class BeatManagerMonoBase<TBeatEnum> : MonoBehaviour, IBeatObservable, IBeatController<TBeatEnum> where TBeatEnum : Enum
    {
        [Header("MakeBeat")] [SerializeField] private MornSerializableDictionaryProvider<TBeatEnum, BeatMemoSo> _beatDictionary;
        private int _nextBeatIndex;
        private BeatMemoSo _memo;
        private float _lastBgmTime;
        private readonly Subject<int> _beatSubject = new();
        private readonly Subject<Unit> _endBeatSubject = new();
        public IObservable<int> OnBeat => _beatSubject;
        public IObservable<Unit> OnEndBeat => _endBeatSubject;

        //次の小節までの時間
        public float LeftTime { get; private set; }

        public void MyUpdate(float time)
        {
            if (_memo == null)
            {
                return;
            }

            if (time < _lastBgmTime)
            {
                _nextBeatIndex = 0;
                _beatSubject.OnNext(0);
            }

            _lastBgmTime = time;
            LeftTime = _memo.GetBeatTiming(Mathf.FloorToInt(_nextBeatIndex / 8f) * 8 + 7) - _lastBgmTime;
            if (_lastBgmTime < _memo.GetBeatTiming(_nextBeatIndex))
            {
                return;
            }

            _beatSubject.OnNext(_nextBeatIndex + 1);
            _nextBeatIndex++;
            if (_nextBeatIndex == _memo.Timings)
            {
                _endBeatSubject.OnNext(Unit.Default);
            }
        }

        public void BeatStart(TBeatEnum beatType)
        {
            _nextBeatIndex = 0;
            _memo = _beatDictionary[beatType];
        }
    }
}

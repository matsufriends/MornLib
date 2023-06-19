using System;
using UniRx;
using UnityEngine;

namespace MornBeat
{
    public static class MornBeatCore<TBeatType> where TBeatType : Enum
    {
        private static MornBeatMemoSo s_currentMemo;
        private static int s_nextBeatIndex;
        private static float s_lastBgmTime;
        private static bool s_waitLoop;
        private static readonly Subject<BeatTimingInfo> s_beatSubject = new();
        private static readonly Subject<Unit> s_endBeatSubject = new();
        public static float LeftMeasureTime { get; private set; }
        public static IObservable<BeatTimingInfo> OnBeat => s_beatSubject;
        public static IObservable<Unit> OnEndBeat => s_endBeatSubject;

        public static void InitializeBeat(TBeatType beatType)
        {
            var solver = MornBeatSolverBase<TBeatType>.Instance;
            s_nextBeatIndex = 0;
            s_currentMemo = solver[beatType];
            s_waitLoop = false;
            solver.OnInitialized(beatType, s_currentMemo.Clip);
        }

        public static void UpdateBeat()
        {
            var solver = MornBeatSolverBase<TBeatType>.Instance;
            var time = solver.PlayingTime;
            if (s_currentMemo == null)
            {
                return;
            }

            if (s_waitLoop)
            {
                if (s_lastBgmTime <= time)
                {
                    return;
                }

                s_waitLoop = false;
            }

            s_lastBgmTime = time;
            LeftMeasureTime = s_currentMemo.GetBeatTiming(Mathf.FloorToInt(s_nextBeatIndex / 8f) * 8 + 7) - s_lastBgmTime;
            if (s_lastBgmTime < s_currentMemo.GetBeatTiming(s_nextBeatIndex))
            {
                return;
            }

            s_beatSubject.OnNext(new BeatTimingInfo(s_nextBeatIndex, s_currentMemo.BeatCount));
            s_waitLoop = s_currentMemo.GetBeatTiming(s_nextBeatIndex) > s_currentMemo.GetBeatTiming(s_nextBeatIndex + 1);
            s_nextBeatIndex++;
            if (s_nextBeatIndex == s_currentMemo.Timings)
            {
                s_nextBeatIndex = 0;
                s_waitLoop = false;
                s_endBeatSubject.OnNext(Unit.Default);
            }
        }
    }
}

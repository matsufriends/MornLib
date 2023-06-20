using System;
using UniRx;

namespace MornBeat
{
    public static class MornBeatCore
    {
        private static MornBeatMemoSo s_currentBeatMemo;
        private static int s_nextBeatIndex;
        private static float s_lastBgmTime;
        private static bool s_waitLoop;
        private static readonly Subject<BeatTimingInfo> s_beatSubject = new();
        private static readonly Subject<Unit> s_endBeatSubject = new();
        public static IObservable<BeatTimingInfo> OnBeat => s_beatSubject;
        public static IObservable<Unit> OnEndBeat => s_endBeatSubject;

        public static void UpdateBeat<TBeatType>() where TBeatType : Enum
        {
            var solver = MornBeatSolverMonoBase<TBeatType>.Instance;
            var time = solver.MusicPlayingTimeImpl;
            if (s_currentBeatMemo == null)
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
            if (s_lastBgmTime < s_currentBeatMemo.GetBeatTiming(s_nextBeatIndex))
            {
                return;
            }

            s_beatSubject.OnNext(new BeatTimingInfo(s_nextBeatIndex, s_currentBeatMemo.BeatCount));
            s_waitLoop = s_currentBeatMemo.GetBeatTiming(s_nextBeatIndex) > s_currentBeatMemo.GetBeatTiming(s_nextBeatIndex + 1);
            s_nextBeatIndex++;
            if (s_nextBeatIndex == s_currentBeatMemo.Timings)
            {
                s_nextBeatIndex = 0;
                s_waitLoop = false;
                s_endBeatSubject.OnNext(Unit.Default);
            }
        }

        public static void InitializeBeat<TBeatType>(TBeatType beatType) where TBeatType : Enum
        {
            var solver = MornBeatSolverMonoBase<TBeatType>.Instance;
            s_nextBeatIndex = 0;
            s_currentBeatMemo = solver[beatType];
            s_waitLoop = false;
            solver.OnInitializeBeatImpl(beatType);
        }
    }
}

using System;
using UniRx;
using UnityEngine;

namespace MornBeat
{
    public static class MornBeatCore
    {
        private static MornBeatMemoSo s_currentBeatMemo;
        private static int s_tick;
        private static float s_lastBgmTime;
        private static bool s_waitLoop;
        private static readonly Subject<BeatTimingInfo> s_beatSubject = new();
        private static readonly Subject<Unit> s_initializeBeatSubject = new();
        private static readonly Subject<Unit> s_endBeatSubject = new();
        public static IObservable<BeatTimingInfo> OnBeat => s_beatSubject;
        public static IObservable<Unit> OnInitializeBeat => s_initializeBeatSubject;
        public static IObservable<Unit> OnEndBeat => s_endBeatSubject;

        public static float GetMusicPlayingTime<TBeatType>() where TBeatType : Enum =>
            MornBeatSolverMonoBase<TBeatType>.Instance.MusicPlayingTimeImpl + s_currentBeatMemo.Offset;

        public static float GetBeatTiming(int tick)
        {
            if (s_currentBeatMemo == null)
            {
                return Mathf.Infinity;
            }

            return s_currentBeatMemo.GetBeatTiming(tick);
        }

        public static void UpdateBeat<TBeatType>() where TBeatType : Enum
        {
            var time = GetMusicPlayingTime<TBeatType>();
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
            if (s_lastBgmTime < s_currentBeatMemo.GetBeatTiming(s_tick))
            {
                return;
            }

            s_beatSubject.OnNext(new BeatTimingInfo(s_tick, s_currentBeatMemo.BeatCount));
            s_waitLoop = s_currentBeatMemo.GetBeatTiming(s_tick) > s_currentBeatMemo.GetBeatTiming(s_tick + 1);
            s_tick++;
            if (s_tick == s_currentBeatMemo.TickSum)
            {
                s_tick = 0;
                s_waitLoop = false;
                s_endBeatSubject.OnNext(Unit.Default);
            }
        }

        public static void InitializeBeat<TBeatType>(TBeatType beatType) where TBeatType : Enum
        {
            var solver = MornBeatSolverMonoBase<TBeatType>.Instance;
            s_tick = 0;
            s_currentBeatMemo = solver[beatType];
            s_waitLoop = false;
            solver.OnInitializeBeatImpl(beatType);
            s_initializeBeatSubject.OnNext(Unit.Default);
        }
    }
}

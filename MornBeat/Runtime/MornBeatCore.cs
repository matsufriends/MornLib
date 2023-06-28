using System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace MornBeat
{
    public static class MornBeatCore
    {
        private static MornBeatMemoSo s_currentBeatMemo;
        private static int s_tick;
        private static bool s_waitLoop;
        private static double s_startDspTime;
        private static Subject<BeatTimingInfo> s_beatSubject = new();
        private static Subject<MornBeatMemoSo> s_initializeBeatSubject = new();
        private static Subject<Unit> s_endBeatSubject = new();
        public static IObservable<BeatTimingInfo> OnBeat => s_beatSubject;
        public static IObservable<MornBeatMemoSo> OnInitializeBeat => s_initializeBeatSubject;
        public static IObservable<Unit> OnEndBeat => s_endBeatSubject;
        public static double OffsetTime;
        public static double CurrentBpm { get; private set; } = 120;
        public static double CurrentBeatLength => 60d / CurrentBpm;
        private static double GetMusicPlayingTime => AudioSettings.dspTime - s_startDspTime + s_currentBeatMemo.Offset + OffsetTime;

        public static void Reset()
        {
            s_currentBeatMemo = null;
            s_tick = 0;
            CurrentBpm = 120;
            s_waitLoop = false;
            s_startDspTime = AudioSettings.dspTime;
            s_beatSubject = new Subject<BeatTimingInfo>();
            s_initializeBeatSubject = new Subject<MornBeatMemoSo>();
            s_endBeatSubject = new Subject<Unit>();
        }

        public static float GetBeatTiming(int tick)
        {
            if (s_currentBeatMemo == null)
            {
                return Mathf.Infinity;
            }

            return s_currentBeatMemo.GetBeatTiming(tick);
        }

        public static void UpdateBeat()
        {
            if (s_currentBeatMemo == null)
            {
                return;
            }

            var time = GetMusicPlayingTime;
            if (s_waitLoop)
            {
                var length = s_currentBeatMemo.Clip.length;
                if (time < length)
                {
                    return;
                }

                s_startDspTime += length;
                time -= length;
                s_waitLoop = false;
            }

            if (time < s_currentBeatMemo.GetBeatTiming(s_tick))
            {
                return;
            }

            CurrentBpm = s_currentBeatMemo.GetBpm(time);
            s_beatSubject.OnNext(new BeatTimingInfo(s_tick, s_currentBeatMemo.BeatCount));
            s_tick++;
            if (s_tick == s_currentBeatMemo.TickSum)
            {
                if (s_currentBeatMemo.IsLoop)
                {
                    s_tick = 0;
                }

                s_waitLoop = true;
                s_endBeatSubject.OnNext(Unit.Default);
            }
        }

        public static void InitializeBeat(MornBeatMemoSo beatMemo, bool isForceInitialize = false)
        {
            var solver = MornBeatSolverMono.Instance;
            if (s_currentBeatMemo == beatMemo && isForceInitialize == false)
            {
                return;
            }

            s_currentBeatMemo = beatMemo;
            s_tick = 0;
            s_waitLoop = false;
            s_startDspTime = AudioSettings.dspTime + 0.1d;
            solver.OnInitializeBeat(beatMemo, s_startDspTime);
            s_initializeBeatSubject.OnNext(beatMemo);
        }

        public static int GetNearTick(out double nearDif)
        {
            return GetNearTickBySpecifiedBeat(out nearDif, s_currentBeatMemo.BeatCount);
        }

        public static int GetNearTickBySpecifiedBeat(out double nearDif, int beat)
        {
            Assert.IsTrue(beat <= s_currentBeatMemo.BeatCount);
            var tickSize = s_currentBeatMemo.BeatCount / beat;
            var lastTick = s_tick - s_tick % tickSize;
            var nextTick = lastTick + tickSize;
            var curTime = GetMusicPlayingTime;
            var preTime = GetBeatTiming(lastTick);
            var nexTime = GetBeatTiming(nextTick);
            while (curTime < preTime && lastTick - tickSize >= 0)
            {
                lastTick -= tickSize;
                nextTick -= tickSize;
                preTime = GetBeatTiming(lastTick);
                nexTime = GetBeatTiming(nextTick);
            }

            while (nexTime < curTime && nextTick + tickSize < s_currentBeatMemo.TickSum)
            {
                lastTick += tickSize;
                nextTick += tickSize;
                preTime = GetBeatTiming(lastTick);
                nexTime = GetBeatTiming(nextTick);
            }

            if (curTime < (preTime + nexTime) / 2f)
            {
                nearDif = preTime - curTime;
                return lastTick;
            }

            nearDif = nexTime - curTime;
            return nextTick;
        }
    }
}
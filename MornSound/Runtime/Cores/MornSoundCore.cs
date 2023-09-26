using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace MornSound
{
    public static class MornSoundCore
    {
        private static MornSoundPlayer s_cachedBgmPlayer;
        private static MornSoundInfo s_lastBgmInfo;
        public static IMornSoundParameter SoundParameter { get; private set; }

        static MornSoundCore()
        {
            OverrideSoundParameter(new MornSoundParameter());
        }

        public static void OverrideSoundParameter(IMornSoundParameter soundParameter)
        {
            SoundParameter = soundParameter;
        }

        public static UniTask FadeInAsync<T>(MornSoundVolumeType volumeType, double duration, CancellationToken token) where T : Enum
        {
            return MornSoundSolverMonoBase<T>.Instance.FadeInAsync(volumeType, (float)duration, token);
        }

        public static UniTask FadeInAsync<T>(MornSoundVolumeType volumeType, float duration, CancellationToken token) where T : Enum
        {
            return MornSoundSolverMonoBase<T>.Instance.FadeInAsync(volumeType, duration, token);
        }

        public static UniTask FadeOutAsync<T>(MornSoundVolumeType volumeType, double duration, CancellationToken token) where T : Enum
        {
            return MornSoundSolverMonoBase<T>.Instance.FadeOutAsync(volumeType, (float)duration, token);
        }

        public static UniTask FadeOutAsync<T>(MornSoundVolumeType volumeType, float duration, CancellationToken token) where T : Enum
        {
            return MornSoundSolverMonoBase<T>.Instance.FadeOutAsync(volumeType, duration, token);
        }

        public static void PlaySe<T>(T soundType) where T : Enum
        {
            var solver = MornSoundSolverMonoBase<T>.Instance;
            var info = solver.GetInfo(soundType);
            var soundPlayer = MornSoundPlayer.GetInstance(solver.transform);
            soundPlayer.Init(solver.SeMixer, info.AudioClip, -16, false, 1, info.IsRandomPitch ? SoundParameter.GetRandomPitch() : 1f);
        }

        public static void PlayBgm<T>(T soundType, bool skipSameTransition = true) where T : Enum
        {
            var solver = MornSoundSolverMonoBase<T>.Instance;
            var info = solver.GetInfo(soundType);
            if (skipSameTransition && s_lastBgmInfo.AudioClip == info.AudioClip)
            {
                return;
            }

            var soundPlayer = MornSoundPlayer.GetInstance(solver.transform);
            soundPlayer.Init(solver.BgmMixer, info.AudioClip, 16, true, 0, 1f);
            soundPlayer.FadeIn(SoundParameter.BgmChangeSeconds);
            if (s_cachedBgmPlayer)
            {
                s_cachedBgmPlayer.FadeOut(SoundParameter.BgmChangeSeconds);
            }

            s_cachedBgmPlayer = soundPlayer;
            s_lastBgmInfo = info;
        }
    }
}
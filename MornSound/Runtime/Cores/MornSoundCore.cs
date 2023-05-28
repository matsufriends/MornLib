using System;
using UniRx;

namespace MornSound
{
    public static class MornSoundCore
    {
        private static MornSoundPlayer s_cachedBgmPlayer;
        private static readonly Subject<float> s_masterVolumeChanged = new();
        private static readonly Subject<float> s_bgmVolumeChanged = new();
        private static readonly Subject<float> s_seVolumeChanged = new();
        public static IObservable<float> OnMasterVolumeChanged => s_masterVolumeChanged;
        public static IObservable<float> OnBgmVolumeChanged => s_bgmVolumeChanged;
        public static IObservable<float> OnSeVolumeChanged => s_seVolumeChanged;
        public static IMornSoundParameter SoundParameter { get; private set; }

        static MornSoundCore()
        {
            OverrideSoundParameter(new MornSoundParameter());
        }

        public static void OverrideSoundParameter(IMornSoundParameter soundParameter)
        {
            SoundParameter = soundParameter;
        }

        public static void PlaySe<T>(T soundType) where T : Enum
        {
            var solver = MornSoundSolverMonoBase<T>.Instance;
            var info = solver.GetInfo(soundType);
            var soundPlayer = MornSoundPlayer.GetInstance(solver.transform);
            soundPlayer.Init(solver.SeMixer, info.AudioClip, false, 1, info.IsRandomPitch ? SoundParameter.GetRandomPitch() : 1f);
        }

        public static void PlayBgm<T>(T soundType) where T : Enum
        {
            var solver = MornSoundSolverMonoBase<T>.Instance;
            var info = solver.GetInfo(soundType);
            var soundPlayer = MornSoundPlayer.GetInstance(solver.transform);
            soundPlayer.Init(solver.BgmMixer, info.AudioClip, true, 0, 1f);
            soundPlayer.FadeIn(SoundParameter.BgmChangeSeconds);
            if (s_cachedBgmPlayer)
            {
                s_cachedBgmPlayer.FadeOut(SoundParameter.BgmChangeSeconds);
            }

            s_cachedBgmPlayer = soundPlayer;
        }
    }
}

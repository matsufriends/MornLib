using System;
using UniRx;

namespace MornSound
{
    public static class MornSoundCore
    {
        private static MornSoundPlayer s_cachedBgmPlayer;
        private static IMornSoundParameter s_parameter = new MornSoundParameter();
        private static readonly Subject<MornSoundVolumeChangeInfo> s_changeVolumeSubject = new();
        public static IObservable<MornSoundVolumeChangeInfo> OnVolumeChanged => s_changeVolumeSubject;

        static MornSoundCore()
        {
            OverrideSoundParameter(new MornSoundParameter());
        }

        public static void ChangeVolume(MornSoundVolumeType volumeType, float volumeRate)
        {
            s_parameter.SaveVolumeRate(volumeType, volumeRate);
            ApplyVolumeRate(volumeType, volumeRate);
        }

        private static void ApplyVolumeRate(MornSoundVolumeType volumeType, float volumeRate)
        {
            var decibel = s_parameter.VolumeRateToDecibel(volumeRate);
            var info = new MornSoundVolumeChangeInfo(volumeType, volumeRate, decibel);
            s_changeVolumeSubject.OnNext(info);
        }

        public static MornSoundVolumeChangeInfo GetVolumeInfo(MornSoundVolumeType volumeType)
        {
            var rate = s_parameter.LoadVolumeRate(volumeType);
            return new MornSoundVolumeChangeInfo(volumeType, rate, s_parameter.VolumeRateToDecibel(rate));
        }

        public static void OverrideSoundParameter(IMornSoundParameter soundParameter)
        {
            s_parameter = soundParameter;
            ApplyVolumeRate(MornSoundVolumeType.Bgm, s_parameter.LoadVolumeRate(MornSoundVolumeType.Bgm));
            ApplyVolumeRate(MornSoundVolumeType.Se, s_parameter.LoadVolumeRate(MornSoundVolumeType.Se));
            ApplyVolumeRate(MornSoundVolumeType.Master, s_parameter.LoadVolumeRate(MornSoundVolumeType.Master));
        }

        public static void PlaySe<T>(T soundType) where T : Enum
        {
            var solver = MornSoundSolverMonoBase<T>.Instance;
            var info = solver.GetInfo(soundType);
            var soundPlayer = MornSoundPlayer.GetInstance(solver.transform);
            soundPlayer.Init(solver.SeMixer, info.AudioClip, false, 1, info.IsRandomPitch ? s_parameter.GetRandomPitch() : 1f);
        }

        public static void PlayBgm<T>(T soundType) where T : Enum
        {
            var solver = MornSoundSolverMonoBase<T>.Instance;
            var info = solver.GetInfo(soundType);
            var soundPlayer = MornSoundPlayer.GetInstance(solver.transform);
            soundPlayer.Init(solver.BgmMixer, info.AudioClip, true, 0, 1f);
            soundPlayer.FadeIn(s_parameter.BgmChangeSeconds);
            if (s_cachedBgmPlayer)
            {
                s_cachedBgmPlayer.FadeOut(s_parameter.BgmChangeSeconds);
            }

            s_cachedBgmPlayer = soundPlayer;
        }
    }
}

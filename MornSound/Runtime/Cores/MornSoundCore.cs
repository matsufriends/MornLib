using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace MornSound
{
    public sealed class MornSoundCore<T> where T : Enum
    {
        private readonly MornSoundSolverMonoBase<T> _solver;
        private MornSoundPlayer _cachedBgmPlayer;
        private MornSoundInfo _lastBgmInfo;

        public MornSoundCore(MornSoundSolverMonoBase<T> solver)
        {
            _solver = solver;
        }

        public void SetSoundParameter(MornSoundParameter soundParameter)
        {
            _solver.SetSoundParameter(soundParameter);
        }

        public UniTask FadeInAsync(MornSoundVolumeType volumeType, double duration, CancellationToken token)
        {
            return _solver.FadeInAsync(volumeType, (float)duration, token);
        }

        public UniTask FadeInAsync(MornSoundVolumeType volumeType, float duration, CancellationToken token)
        {
            return _solver.FadeInAsync(volumeType, duration, token);
        }

        public UniTask FadeOutAsync(MornSoundVolumeType volumeType, double duration, CancellationToken token)
        {
            return _solver.FadeOutAsync(volumeType, (float)duration, token);
        }

        public UniTask FadeOutAsync(MornSoundVolumeType volumeType, float duration, CancellationToken token)
        {
            return _solver.FadeOutAsync(volumeType, duration, token);
        }

        public void PlaySe(T soundType, float volume = 1)
        {
            var info = _solver.GetInfo(soundType);
            var soundPlayer = MornSoundPlayer.GetInstance(_solver.transform);
            var pitch = info.IsRandomPitch ? _solver.SoundParameter.GetRandomPitch() : 1f;
            soundPlayer.Init(_solver.SeMixer, info.AudioClip, -16, false, volume, pitch);
        }

        public void PlayBgm(T soundType, float volume = 1, float fadeDuration = 1, bool skipSameTransition = true)
        {
            var info = _solver.GetInfo(soundType);
            if (skipSameTransition && _lastBgmInfo.AudioClip == info.AudioClip)
            {
                return;
            }

            var soundPlayer = MornSoundPlayer.GetInstance(_solver.transform);
            soundPlayer.Init(_solver.BgmMixer, info.AudioClip, 16, true, volume, 1f);
            soundPlayer.FadeIn(fadeDuration);
            if (_cachedBgmPlayer)
            {
                _cachedBgmPlayer.FadeOut(fadeDuration);
            }

            _cachedBgmPlayer = soundPlayer;
            _lastBgmInfo = info;
        }

        public void StopBgm(float fadeDuration = 1)
        {
            if (_cachedBgmPlayer)
            {
                _cachedBgmPlayer.FadeOut(fadeDuration);
            }

            _cachedBgmPlayer = null;
            _lastBgmInfo = default(MornSoundInfo);
        }

        public void Reset()
        {
            _cachedBgmPlayer = null;
            _lastBgmInfo = default(MornSoundInfo);
        }
    }
}
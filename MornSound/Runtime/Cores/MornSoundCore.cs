using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornSound
{
    public sealed class MornSoundCore
    {
        private readonly MornSoundSolverMonoBase _solver;
        private MornSoundPlayer _cachedBgmPlayer;
        private MornSoundDataSo _lastBgmInfo;
        private readonly Dictionary<MornSoundVolumeType, float> _fadeScaleDict = new()
        {
                { MornSoundVolumeType.Master, 1 },
                { MornSoundVolumeType.Bgm, 1 },
                { MornSoundVolumeType.Se, 1 },
        };
        private CancellationTokenSource _cancellationTokenSource;
        private const string MasterVolumeKey = "MasterVolume";
        private const string SeVolumeKey = "SeVolume";
        private const string BGMVolumeKey = "BgmVolume";

        public MornSoundCore(MornSoundSolverMonoBase solver)
        {
            _solver = solver;
            _solver.Initialize(ApplyVolumeToMixer);
        }

        public void PlaySe(MornSoundDataSo info, float volume = 1)
        {
            var soundPlayer = MornSoundPlayer.GetInstance(_solver.transform);
            soundPlayer.Init(_solver.SeGroup, info.AudioClip, -16, false, volume * info.VolumeRate, info.PitchRate);
        }

        public void PlayBgm(MornSoundDataSo info, float volume = 1, float fadeDuration = 1, bool skipSameTransition = true, double? scheduled = null)
        {
            if (skipSameTransition && _lastBgmInfo != null && _lastBgmInfo.AudioClip == info.AudioClip)
            {
                return;
            }

            var soundPlayer = MornSoundPlayer.GetInstance(_solver.transform);
            soundPlayer.Init(_solver.BgmGroup, info.AudioClip, 16, true, volume * info.VolumeRate, info.PitchRate, scheduled);
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
            _lastBgmInfo = null;
        }

        public async UniTask FadeInAsync(MornSoundVolumeType volumeType, float duration, CancellationToken token)
        {
            await FadeInternalAsync(volumeType, duration, 1, token);
        }

        public async UniTask FadeOutAsync(MornSoundVolumeType volumeType, float duration, CancellationToken token)
        {
            await FadeInternalAsync(volumeType, duration, 0, token);
        }

        private async UniTask FadeInternalAsync(MornSoundVolumeType volumeType, float duration, float endValue, CancellationToken token)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            var elapsedTime = 0f;
            var start = _fadeScaleDict[volumeType];
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _fadeScaleDict[volumeType] = Mathf.Lerp(start, endValue, Mathf.Clamp01(elapsedTime / duration));
                ApplyVolumeToMixer(volumeType);
                await UniTask.Yield(PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            }
            
            _fadeScaleDict[volumeType] = endValue;
            ApplyVolumeToMixer(volumeType);
        }

        public void ApplyVolumeToMixer(MornSoundVolumeType volumeType)
        {
            var baseVolume = volumeType switch
            {
                    MornSoundVolumeType.Master => _solver.MasterVolume,
                    MornSoundVolumeType.Bgm    => _solver.BgmVolume,
                    MornSoundVolumeType.Se     => _solver.SeVolume,
                    _                          => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
            var fadeScale = _fadeScaleDict[volumeType];
            var volumeKey = volumeType switch
            {
                    MornSoundVolumeType.Master => MasterVolumeKey,
                    MornSoundVolumeType.Se     => SeVolumeKey,
                    MornSoundVolumeType.Bgm    => BGMVolumeKey,
                    _                          => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
            _solver.Mixer.SetFloat(volumeKey, VolumeRateToDecibel(baseVolume * fadeScale));
        }

        private static float VolumeRateToDecibel(float rate)
        {
            return rate <= 0 ? -5000 : (1 - rate) * -30;
        }
    }
}
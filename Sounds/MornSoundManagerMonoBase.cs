using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornLib.Cores;
using MornLib.Singletons;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

namespace MornLib.Sounds
{
    public abstract class MornSoundManagerMonoBase<TEnum> : MornSingletonMono<MornSoundManagerMonoBase<TEnum>> where TEnum : Enum
    {
        [SerializeField] private MornSerializableDictionaryProvider<TEnum, AudioClip> _soundClipDictionaryProvider;
        [SerializeField] private AudioSource _bgmSourceA;
        [SerializeField] private AudioSource _bgmSourceB;
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private AudioMixer _mixer;
        private bool _isPlayingBgmOnSourceA;
        private CancellationTokenSource _cachedBgmFadeTokenSource;
        private IMornSoundSaver _soundSaver = new MornSoundSaver();
        public float Time => _isPlayingBgmOnSourceA ? _bgmSourceA.time : _bgmSourceB.time;
        private const float DefaultVolume = 0.1f;
        private const string MasterMixerKey = "MasterVolume";
        private const string SeMixerKey = "SeVolume";
        private const string BGMMixerKey = "BgmVolume";
        private const float MinDb = 30;

        protected override async void MyAwake()
        {
            _bgmSourceA.loop = true;
            _bgmSourceB.loop = true;
            await UniTask.Yield(PlayerLoopTiming.LastInitialization);
            var masterVolume = _soundSaver.LoadVolume(MornSoundSliderType.Master, DefaultVolume);
            var seVolume = _soundSaver.LoadVolume(MornSoundSliderType.Se, DefaultVolume);
            var bgmVolume = _soundSaver.LoadVolume(MornSoundSliderType.Bgm, DefaultVolume);
            _mixer.SetFloat(MasterMixerKey, RateToDb(masterVolume));
            _mixer.SetFloat(SeMixerKey, RateToDb(seVolume));
            _mixer.SetFloat(BGMMixerKey, RateToDb(bgmVolume));
        }

        public void PlayBgm(TEnum soundType, TimeSpan duration)
        {
            PlayBgm(_soundClipDictionaryProvider.GetDictionary()[soundType], duration);
        }

        public void PlayBgm(AudioClip clip, TimeSpan duration)
        {
            _cachedBgmFadeTokenSource?.Cancel();
            _cachedBgmFadeTokenSource?.Dispose();
            _cachedBgmFadeTokenSource = new CancellationTokenSource();
            var fadeInSource = _isPlayingBgmOnSourceA ? _bgmSourceB : _bgmSourceA;
            var fadeOutSource = _isPlayingBgmOnSourceA ? _bgmSourceA : _bgmSourceB;
            fadeInSource.clip = clip;
            fadeInSource.Play();
            MornTask.TransitionAsync(duration, true, rate =>
                {
                    fadeInSource.volume = rate;
                    fadeOutSource.volume = 1 - rate;
                }, _cachedBgmFadeTokenSource.Token)
                .Forget();
            _isPlayingBgmOnSourceA = !_isPlayingBgmOnSourceA;
        }

        public void PlaySe(TEnum soundType)
        {
            var clip = _soundClipDictionaryProvider.GetDictionary()[soundType];
            _seSource.PlayOneShot(clip);
        }

        public void SetSoundSaver(IMornSoundSaver soundSaver)
        {
            _soundSaver = soundSaver;
        }

        public void RegisterSlider(MornSoundSliderMonoBase<TEnum> slider)
        {
            var mixerKey = slider.MornSoundSliderType switch
            {
                MornSoundSliderType.Master => MasterMixerKey, MornSoundSliderType.Se => SeMixerKey,
                MornSoundSliderType.Bgm => BGMMixerKey, _ => "",
            };
            slider.SetValue(_soundSaver.LoadVolume(slider.MornSoundSliderType, DefaultVolume));
            slider.OnValueChanged.Subscribe(x =>
                {
                    _soundSaver.SaveVolume(slider.MornSoundSliderType, x);
                    _mixer.SetFloat(mixerKey, RateToDb(x));
                })
                .AddTo(this);
        }

        private static float RateToDb(float rate)
        {
            return rate <= 0 ? -5000 : (rate - 1) * MinDb;
        }
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornLib.Cores;
using MornLib.Singletons;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
namespace MornLib.Sounds {
    public abstract class BaseMornSoundManagerMono<TEnum> : SingletonMono<BaseMornSoundManagerMono<TEnum>> where TEnum : Enum {
        [SerializeField] private MornSerializableDictionaryProvider<TEnum,AudioClip> _soundClipDictionaryProvider;
        [SerializeField] private AudioSource _bgmSourceA;
        [SerializeField] private AudioSource _bgmSourceB;
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private AudioMixer _mixer;
        private bool _isPlayingBgmOnSourceA;
        private CancellationTokenSource _cachedBgmFadeTokenSource;
        private const string c_masterVolumeKey = "MasterVolume";
        private const string c_seVolume = "SeVolume";
        private const string c_bgmVolume = "BgmVolume";
        private const float c_minDb = 30;

        protected override async void MyAwake() {
            _bgmSourceA.loop = true;
            _bgmSourceB.loop = true;
            await UniTask.Yield(PlayerLoopTiming.LastInitialization);
            _mixer.SetFloat(c_masterVolumeKey,RateToDb(PlayerPrefs.GetFloat(c_masterVolumeKey,1)));
            _mixer.SetFloat(c_seVolume,RateToDb(PlayerPrefs.GetFloat(c_seVolume,1)));
            _mixer.SetFloat(c_bgmVolume,RateToDb(PlayerPrefs.GetFloat(c_bgmVolume,1)));
        }

        public void PlayBgm(TEnum soundType,TimeSpan duration) {
            _cachedBgmFadeTokenSource?.Cancel();
            _cachedBgmFadeTokenSource?.Dispose();
            _cachedBgmFadeTokenSource = new CancellationTokenSource();
            var fadeInSource = _isPlayingBgmOnSourceA ? _bgmSourceB : _bgmSourceA;
            var fadeOutSource = _isPlayingBgmOnSourceA ? _bgmSourceA : _bgmSourceB;
            fadeInSource.clip = _soundClipDictionaryProvider.GetDictionary()[soundType];
            fadeInSource.Play();
            MornTask.TransitionAsync(
                duration,true,rate => {
                    fadeInSource.volume  = rate;
                    fadeOutSource.volume = 1 - rate;
                },_cachedBgmFadeTokenSource.Token
            ).Forget();
            _isPlayingBgmOnSourceA = !_isPlayingBgmOnSourceA;
        }

        public void PlaySe(TEnum soundType) {
            var clip = _soundClipDictionaryProvider.GetDictionary()[soundType];
            _seSource.PlayOneShot(clip);
        }

        public void InitSlider(BaseMornSoundSliderMono<TEnum> slider) {
            var key = slider.SoundSliderType switch {
                SoundSliderType.Master => c_masterVolumeKey
               ,SoundSliderType.Se     => c_seVolume
               ,SoundSliderType.Bgm    => c_bgmVolume
               ,_                      => ""
            };
            slider.SetValue(PlayerPrefs.GetFloat(key,1));
            slider.OnValueChanged.Subscribe(
                x => {
                    PlayerPrefs.SetFloat(key,x);
                    _mixer.SetFloat(key,RateToDb(x));
                    PlayerPrefs.Save();
                }
            ).AddTo(this);
        }

        private static float RateToDb(float rate) {
            return rate <= 0 ? -5000 : (rate - 1) * c_minDb;
        }
    }
}

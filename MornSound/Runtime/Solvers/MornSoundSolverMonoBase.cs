using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornSetting;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

namespace MornSound
{
    public abstract class MornSoundSolverMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        //変数名はEditor拡張よりシリアライズされるので要注意
        [SerializeField] private List<TEnum> _keyList;
        [SerializeField] private List<AudioClip> _clipList;
        [SerializeField] private List<bool> _isRandomPitchList;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioMixerGroup _seMixer;
        [SerializeField] private AudioMixerGroup _bgmMixer;
        [SerializeField] private MornSettingFloatSo _masterSo;
        [SerializeField] private MornSettingFloatSo _bgmSo;
        [SerializeField] private MornSettingFloatSo _seSo;
        private Dictionary<TEnum, int> _enumToIndexDictionary;
        private float _materVolumeFadeRate = 1;
        private float _seVolumeFadeRate = 1;
        private float _bgmVolumeFadeRate = 1;
        private const string MasterVolumeKey = "MasterVolume";
        private const string SeVolumeKey = "SeVolume";
        private const string BGMVolumeKey = "BgmVolume";
        public AudioMixerGroup SeMixer => _seMixer;
        public AudioMixerGroup BgmMixer => _bgmMixer;
        private static MornSoundSolverMonoBase<TEnum> s_instance;
        public static MornSoundSolverMonoBase<TEnum> Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornSoundSolverMonoBase<TEnum>>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornSoundSolverMonoBase<TEnum>)} is not found.");
                }

                return s_instance;
            }
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (s_instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _masterSo.OnFloatChanged.Subscribe(x => ApplyVolumeToMixer(MornSoundVolumeType.Master, x)).AddTo(this);
            _bgmSo.OnFloatChanged.Subscribe(x => ApplyVolumeToMixer(MornSoundVolumeType.Bgm, x)).AddTo(this);
            _seSo.OnFloatChanged.Subscribe(x => ApplyVolumeToMixer(MornSoundVolumeType.Se, x)).AddTo(this);
        }

        private void Start()
        {
            //Mixer.SetFloatがAwake関数では適切に処理されない
            ApplyVolumeToMixer(MornSoundVolumeType.Master, _masterSo.LoadFloat());
            ApplyVolumeToMixer(MornSoundVolumeType.Se, _seSo.LoadFloat());
            ApplyVolumeToMixer(MornSoundVolumeType.Bgm, _bgmSo.LoadFloat());
        }

        internal async UniTask FadeInAsync(MornSoundVolumeType volumeType, float duration, CancellationToken token)
        {
            var elapsedTime = 0f;
            while (elapsedTime <= duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                var rate = Mathf.Clamp01(elapsedTime / duration);
                UpdateFadeVolume(volumeType, rate);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            UpdateFadeVolume(volumeType, 1);
        }

        internal async UniTask FadeOutAsync(MornSoundVolumeType volumeType, float duration, CancellationToken token)
        {
            var elapsedTime = 0f;
            while (elapsedTime <= duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                var rate = Mathf.Clamp01(elapsedTime / duration);
                UpdateFadeVolume(volumeType, 1 - rate);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            UpdateFadeVolume(volumeType, 0);
        }

        private void UpdateFadeVolume(MornSoundVolumeType volumeType, float value)
        {
            switch (volumeType)
            {
                case MornSoundVolumeType.Master:
                    _materVolumeFadeRate = value;
                    ApplyVolumeToMixer(volumeType, _masterSo.LoadFloat());
                    break;
                case MornSoundVolumeType.Bgm:
                    _bgmVolumeFadeRate = value;
                    ApplyVolumeToMixer(volumeType, _bgmSo.LoadFloat());
                    break;
                case MornSoundVolumeType.Se:
                    _seVolumeFadeRate = value;
                    ApplyVolumeToMixer(volumeType, _seSo.LoadFloat());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null);
            }
        }

        private void ApplyVolumeToMixer(MornSoundVolumeType volumeType, float value)
        {
            var volumeRate = volumeType switch
            {
                MornSoundVolumeType.Master => _materVolumeFadeRate,
                MornSoundVolumeType.Se     => _seVolumeFadeRate,
                MornSoundVolumeType.Bgm    => _bgmVolumeFadeRate,
                _                          => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
            var volumeKey = volumeType switch
            {
                MornSoundVolumeType.Master => MasterVolumeKey,
                MornSoundVolumeType.Se     => SeVolumeKey,
                MornSoundVolumeType.Bgm    => BGMVolumeKey,
                _                          => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
            _mixer.SetFloat(volumeKey, MornSoundCore.SoundParameter.VolumeRateToDecibel(value * volumeRate));
        }

        public MornSoundInfo GetInfo(TEnum value)
        {
            if (_enumToIndexDictionary == null)
            {
                _enumToIndexDictionary = new Dictionary<TEnum, int>();
                for (var i = 0; i < _keyList.Count; i++)
                {
                    _enumToIndexDictionary.Add(_keyList[i], i);
                }
            }

            if (_enumToIndexDictionary.TryGetValue(value, out var index))
            {
                return new MornSoundInfo(_clipList[index], _isRandomPitchList[index]);
            }

            Debug.LogError($"Key:{value} is not found.");
            return default(MornSoundInfo);
        }
    }
}

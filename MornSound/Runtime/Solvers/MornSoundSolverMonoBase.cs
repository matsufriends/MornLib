using System;
using System.Collections.Generic;
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

            _masterSo.OnFloatChanged.Subscribe(x => ApplyVolume(MornSoundVolumeType.Master, x)).AddTo(this);
            _bgmSo.OnFloatChanged.Subscribe(x => ApplyVolume(MornSoundVolumeType.Bgm, x)).AddTo(this);
            _seSo.OnFloatChanged.Subscribe(x => ApplyVolume(MornSoundVolumeType.Se, x)).AddTo(this);
        }

        private void Start()
        {
            //Mixer.SetFloatがAwake関数では適切に処理されない
            ApplyVolume(MornSoundVolumeType.Master, _masterSo.LoadFloat());
            ApplyVolume(MornSoundVolumeType.Se, _seSo.LoadFloat());
            ApplyVolume(MornSoundVolumeType.Bgm, _bgmSo.LoadFloat());
        }

        private void ApplyVolume(MornSoundVolumeType volumeType, float value)
        {
            var volumeKey = volumeType switch
            {
                MornSoundVolumeType.Master => MasterVolumeKey,
                MornSoundVolumeType.Se     => SeVolumeKey,
                MornSoundVolumeType.Bgm    => BGMVolumeKey,
                _                          => throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null),
            };
            _mixer.SetFloat(volumeKey, MornSoundCore.SoundParameter.VolumeRateToDecibel(value));
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

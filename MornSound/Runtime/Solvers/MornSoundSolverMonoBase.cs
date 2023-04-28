using System;
using System.Collections.Generic;
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
            }
        }

        private void Start()
        {
            //Mixer.SetFloatがAwake関数では適切に処理されない
            ApplyVolume(MornSoundCore.GetVolumeInfo(MornSoundVolumeType.Master));
            ApplyVolume(MornSoundCore.GetVolumeInfo(MornSoundVolumeType.Se));
            ApplyVolume(MornSoundCore.GetVolumeInfo(MornSoundVolumeType.Bgm));
            MornSoundCore.OnVolumeChanged.Subscribe(ApplyVolume).AddTo(this);
        }

        private void ApplyVolume(MornSoundVolumeChangeInfo info)
        {
            var volumeKey = info.VolumeType switch
            {
                MornSoundVolumeType.Master => MasterVolumeKey,
                MornSoundVolumeType.Se => SeVolumeKey,
                MornSoundVolumeType.Bgm => BGMVolumeKey,
                _ => throw new ArgumentOutOfRangeException(nameof(info.VolumeType), info.VolumeType, null),
            };
            _mixer.SetFloat(volumeKey, info.VolumeDecibel);
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
            return default;
        }
    }
}

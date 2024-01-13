using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MornSound
{
    public abstract class MornSoundSolverMonoBase : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioMixerGroup _bgmGroup;
        [SerializeField] private AudioMixerGroup _seGroup;
        internal AudioMixer Mixer => _mixer;
        internal AudioMixerGroup BgmGroup => _bgmGroup;
        internal AudioMixerGroup SeGroup => _seGroup;
        public abstract float MasterVolume { get; }
        public abstract float BgmVolume { get; }
        public abstract float SeVolume { get; }
        private Action<MornSoundVolumeType> _applyVolume;

        internal void Initialize(Action<MornSoundVolumeType> applyVolume)
        {
            _applyVolume = applyVolume;
        }

        protected virtual void Start()
        {
            //Mixer.SetFloatがAwake関数では適切に処理されない
            _applyVolume(MornSoundVolumeType.Master);
            _applyVolume(MornSoundVolumeType.Bgm);
            _applyVolume(MornSoundVolumeType.Se);
        }
    }
}
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornSound
{
    [RequireComponent(typeof(Slider))]
    internal sealed class MornSoundVolumeSliderMono : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MornSoundVolumeType _mornSoundVolumeType;
        private bool _selfChangeLock;

        private void Awake()
        {
            if (_slider == null)
            {
                _slider = TryGetComponent<Slider>(out var slider) ? slider : gameObject.AddComponent<Slider>();
            }

            ApplyVolume(MornSoundCore.GetVolumeInfo(MornSoundVolumeType.Master));
            ApplyVolume(MornSoundCore.GetVolumeInfo(MornSoundVolumeType.Se));
            ApplyVolume(MornSoundCore.GetVolumeInfo(MornSoundVolumeType.Bgm));

            //スライダーの購読は、値の反映が終わってから行う
            MornSoundCore.OnVolumeChanged.Subscribe(ApplyVolume).AddTo(this);
            _slider.OnValueChangedAsObservable()
                .Subscribe(x =>
                {
                    _selfChangeLock = true;
                    MornSoundCore.ChangeVolume(_mornSoundVolumeType, x);
                    _selfChangeLock = false;
                })
                .AddTo(this);
        }

        private void ApplyVolume(MornSoundVolumeChangeInfo info)
        {
            if (_selfChangeLock == false && info.VolumeType == _mornSoundVolumeType)
            {
                _slider.value = info.VolumeRate;
            }
        }

        private void Reset()
        {
            _slider = GetComponent<Slider>();
        }
    }
}

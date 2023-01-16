using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornLib.Sounds
{
    [RequireComponent(typeof(Slider))]
    public class MornSoundSliderMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MornSoundVolumeType _mornSoundVolumeType;
        public IObservable<float> OnValueChanged => _slider.OnValueChangedAsObservable();
        public MornSoundVolumeType MornSoundVolumeType => _mornSoundVolumeType;

        private void Awake()
        {
            MornSoundManagerMonoBase<TEnum>.Instance.RegisterSlider(this);
        }

        private void Reset()
        {
            _slider = GetComponent<Slider>();
        }

        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}

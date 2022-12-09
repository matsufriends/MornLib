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
        [SerializeField] private MornSoundSliderType _mornSoundSliderType;
        public IObservable<float> OnValueChanged => _slider.OnValueChangedAsObservable();
        public MornSoundSliderType MornSoundSliderType => _mornSoundSliderType;

        private void Awake()
        {
            MornSoundManagerMonoBase<TEnum>.Instance.InitSlider(this);
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

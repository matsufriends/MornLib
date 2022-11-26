using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
namespace MornLib.Sounds {
    [RequireComponent(typeof(Slider))]
    public class BaseMornSoundSliderMono<TEnum> : MonoBehaviour where TEnum : Enum {
        [SerializeField] private Slider _slider;
        [SerializeField] private SoundSliderType _soundSliderType;
        public IObservable<float> OnValueChanged => _slider.OnValueChangedAsObservable();
        public SoundSliderType SoundSliderType => _soundSliderType;

        private void Awake() {
            BaseMornSoundManagerMono<TEnum>.Instance.InitSlider(this);
        }

        private void Reset() {
            _slider = GetComponent<Slider>();
        }

        public void SetValue(float value) {
            _slider.value = value;
        }
    }
}

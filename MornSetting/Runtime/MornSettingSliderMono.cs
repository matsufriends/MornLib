using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornSetting
{
    [RequireComponent(typeof(Slider))]
    public sealed class MornSettingSliderMono : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MornSettingFloatSo _settingSo;
        private bool _selfChangeLock;

        private void Awake()
        {
            ApplyValue(_settingSo.LoadValue());
            _settingSo.OnValueChanged.Where(x => _selfChangeLock == false).Subscribe(ApplyValue).AddTo(this);
            _slider.OnValueChangedAsObservable().Subscribe(
                x =>
                {
                    _selfChangeLock = true;
                    _settingSo.SaveValue(x);
                    _selfChangeLock = false;
                }).AddTo(this);
        }

        private void ApplyValue(float value)
        {
            _slider.value = value;
        }

        private void Reset()
        {
            _slider = GetComponent<Slider>();
        }
    }
}
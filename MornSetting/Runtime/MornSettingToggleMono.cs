using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornSetting
{
    [RequireComponent(typeof(Toggle))]
    public sealed class MornSettingToggleMono : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private MornSettingBoolSo _settingSo;
        private bool _selfChangeLock;

        private void Awake()
        {
            ApplyValue(_settingSo.LoadValue());
            _settingSo.OnValueChanged.Where(x => _selfChangeLock == false).Subscribe(ApplyValue).AddTo(this);
            _toggle.OnValueChangedAsObservable().Subscribe(
                x =>
                {
                    _selfChangeLock = true;
                    _settingSo.SaveValue(x);
                    _selfChangeLock = false;
                }).AddTo(this);
        }

        private void ApplyValue(bool value)
        {
            _toggle.isOn = value;
        }

        private void Reset()
        {
            _toggle = GetComponent<Toggle>();
        }
    }
}
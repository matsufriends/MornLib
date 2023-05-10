using System;
using MornAttribute;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornUI
{
    public sealed class MornUISliderMono : MornUIBehaviourMonoBase
    {
        [Header("Slider")]
        [SerializeField] [ReadOnly] private int _value;
        [SerializeField] private int _maxValue;
        [SerializeField] [Range(0, 1f)] private float _rate;
        [SerializeField] private Image _fillImage;
        [SerializeField] private MornUISliderHandleMono _handle;
        private readonly Subject<int> _valueChangeSubject = new();
        private readonly Subject<float> _rateChangeSubject = new();
        public IObservable<int> OnValueChanged => _valueChangeSubject;
        public IObservable<float> OnRateChanged => _rateChangeSubject;

        private void Awake()
        {
            _handle.OnHandleMove.Subscribe(ChangeValue).AddTo(this);
        }

        private void OnValidate()
        {
            ApplyValue(Mathf.RoundToInt(_rate * _maxValue));
        }

        public override void Selected()
        {
        }

        public override void OnSubmit(out bool canTransition)
        {
            canTransition = true;
        }

        public override void OnMove(MornUIAxisDirType axis, out bool canTransition)
        {
            canTransition = true;
        }

        private void ChangeValue(MornUIAxisDirType axis)
        {
            int dif;
            switch (axis)
            {
                case MornUIAxisDirType.Right:
                    dif = 1;
                    break;
                case MornUIAxisDirType.Left:
                    dif = -1;
                    break;
                default:
                    return;
            }

            ApplyValue(_value + dif);
            _valueChangeSubject.OnNext(_value);
            _rateChangeSubject.OnNext(_rate);
        }

        public void ApplyValue(int newValue)
        {
            var cachedValue = _value;
            _value = Mathf.Clamp(newValue, 0, _maxValue);
            if (cachedValue == _value)
            {
                return;
            }

            _rate = (float)_value / _maxValue;
            if (_fillImage == null)
            {
                return;
            }

            _fillImage.fillAmount = _rate;
            var backRect = _fillImage.rectTransform.rect;
            var lerpT = _fillImage.fillOrigin == 0 ? _rate : 1 - _rate;
            switch (_fillImage.fillMethod)
            {
                case Image.FillMethod.Horizontal:
                    _handle.SetAnchoredPosition(new Vector2(Mathf.Lerp(backRect.xMin, backRect.xMax, lerpT), 0));
                    break;
                case Image.FillMethod.Vertical:
                    _handle.SetAnchoredPosition(new Vector2(0, Mathf.Lerp(backRect.yMin, backRect.yMax, lerpT)));
                    break;
            }
        }
    }
}

using System;
using MornAttribute;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornUI
{
    public sealed class MornUIHorizontalSliderMono : MornUISelectableMonoBase
    {
        [Header("Slider")]
        [SerializeField] [ReadOnly] private int _value;
        [SerializeField] private int _maxValue;
        [SerializeField] [Range(0, 1f)] private float _rate;
        [SerializeField] private Graphic _handleGraphic;
        [SerializeField] private RectTransform _back;
        [SerializeField] private RectTransform _fill;
        private bool _isEditValue;
        private readonly Subject<int> _valueChangeSubject = new();
        private readonly Subject<float> _rateChangeSubject = new();
        public IObservable<int> OnValueChanged => _valueChangeSubject;
        public IObservable<float> OnRateChanged => _rateChangeSubject;

        protected override void OnValidateImpl()
        {
            _value = Mathf.RoundToInt(_rate * _maxValue);
#if UNITY_EDITOR
            EditorApplication.delayCall += Fill;
#endif
        }

#if UNITY_EDITOR
        private void Fill()
        {
            EditorApplication.delayCall -= Fill;
            if (this == null)
            {
                return;
            }

            if (_back == null || _fill == null)
            {
                return;
            }

            _fill.sizeDelta = new Vector2(-_back.sizeDelta.x * (1 - _rate), 0);
        }

#endif

        public override void Submit()
        {
            _isEditValue = !_isEditValue;
            _selectableGraphic.enabled = !_isEditValue;
            _handleGraphic.enabled = _isEditValue;
        }

        protected override void OnSelectedImpl()
        {
            _isEditValue = false;
            _selectableGraphic.enabled = !_isEditValue;
            _handleGraphic.enabled = _isEditValue;
        }

        protected override void OnDeselectedImpl()
        {
            _isEditValue = false;
            _selectableGraphic.enabled = !_isEditValue;
            _handleGraphic.enabled = _isEditValue;
        }

        internal override void Transition(MornUIAxisDirType axis)
        {
            if (_isEditValue)
            {
                switch (axis)
                {
                    case MornUIAxisDirType.Right:
                        ChangeValue(1);
                        break;
                    case MornUIAxisDirType.Left:
                        ChangeValue(-1);
                        break;
                }
            }
            else
            {
                base.Transition(axis);
            }
        }

        private void ChangeValue(int dif)
        {
            _value = Mathf.Clamp(_value + dif, 0, _maxValue);
            _rate = (float)_value / _maxValue;
            _fill.sizeDelta = new Vector2(-_back.sizeDelta.x * (1 - _rate), 0);
            _valueChangeSubject.OnNext(_value);
            _rateChangeSubject.OnNext(_rate);
        }
    }
}

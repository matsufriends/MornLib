using System;
using UniRx;
using UnityEngine;

namespace MornUI
{
    public sealed class MornUIButtonMono : MornUIMonoBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;
        private readonly Subject<bool> _onFocusAsObservableSubject = new();
        private readonly Subject<bool> _onUnfocusAsObservableSubject = new();
        private readonly Subject<Unit> _onSubmitAsObservableSubject = new();
        private Vector3? _defaultScale;
        private const float SubmitScale = 1.1f;
        private const float LerpT = 10f;
        public IObservable<Unit> OnSubmitAsObservable => _onSubmitAsObservableSubject;
        public IObservable<bool> OnFocusAsObservable => _onFocusAsObservableSubject;
        public IObservable<bool> OnUnfocusAsObservable => _onUnfocusAsObservableSubject;

        public override void OnSubmit()
        {
            base.OnSubmit();
            _onSubmitAsObservableSubject.OnNext(Unit.Default);
            DoScale();
        }

        public override void OnFocus(bool isInitialFocus)
        {
            _focused.SetActive(true);
            _unfocused.SetActive(false);
            _onFocusAsObservableSubject.OnNext(isInitialFocus);
            DoScale();
        }

        public override void OnUnFocus(bool isInitialFocus)
        {
            _focused.SetActive(false);
            _unfocused.SetActive(true);
            _onUnfocusAsObservableSubject.OnNext(isInitialFocus);
        }

        private void DoScale()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_defaultScale == null)
            {
                _defaultScale = RectTransform.localScale;
            }

            RectTransform.localScale = _defaultScale.Value * SubmitScale;
        }

        private void Update()
        {
            if (_defaultScale != null)
            {
                var a = RectTransform.localScale;
                var b = _defaultScale.Value;
                var t = Time.deltaTime * LerpT;
                RectTransform.localScale = Vector3.Lerp(a, b, t);
            }
        }
    }
}
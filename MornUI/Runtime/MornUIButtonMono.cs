using System;
using UniRx;
using UnityEngine;

namespace MornUI
{
    public sealed class MornUIButtonMono : MornUIMonoBase
    {
        [Header("Button")]
        [SerializeField] private GameObject focused;
        [SerializeField] private GameObject unfocused;
        private readonly Subject<bool> onFocusAsObservableSubject = new();
        private readonly Subject<bool> onUnfocusAsObservableSubject = new();
        private readonly Subject<Unit> onSubmitAsObservableSubject = new();
        private Vector3? defaultScale;
        private const float SubmitScale = 1.1f;
        private const float LerpT = 10f;
        public IObservable<Unit> OnSubmitAsObservable => onSubmitAsObservableSubject;
        public IObservable<bool> OnFocusAsObservable => onFocusAsObservableSubject;
        public IObservable<bool> OnUnfocusAsObservable => onUnfocusAsObservableSubject;

        public override void OnSubmit()
        {
            base.OnSubmit();
            onSubmitAsObservableSubject.OnNext(Unit.Default);
            DoScale();
        }

        public override void OnFocus(bool isInitialFocus)
        {
            focused.SetActive(true);
            unfocused.SetActive(false);
            onFocusAsObservableSubject.OnNext(isInitialFocus);
            DoScale();
        }

        public override void OnUnFocus(bool isInitialFocus)
        {
            focused.SetActive(false);
            unfocused.SetActive(true);
            onUnfocusAsObservableSubject.OnNext(isInitialFocus);
        }

        private void DoScale()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (defaultScale == null)
            {
                defaultScale = RectTransform.localScale;
            }

            RectTransform.localScale = defaultScale.Value * SubmitScale;
        }

        private void Update()
        {
            if (defaultScale != null)
            {
                var a = RectTransform.localScale;
                var b = defaultScale.Value;
                var t = Time.deltaTime * LerpT;
                RectTransform.localScale = Vector3.Lerp(a, b, t);
            }
        }
    }
}
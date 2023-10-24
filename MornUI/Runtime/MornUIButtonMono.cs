﻿using System;
using UniRx;
using UnityEngine;

namespace MornUI
{
    public sealed class MornUIButtonMono : MornUIMonoBase
    {
        [SerializeField] private GameObject _focused;
        [SerializeField] private GameObject _unfocused;
        private readonly Subject<Unit> _onSubmitAsObservableSubject = new();
        private const float SubmitScale = 1.1f;
        private const float LerpT = 0.1f;

        public IObservable<Unit> OnSubmitAsObservable()
        {
            return _onSubmitAsObservableSubject;
        }

        public override void OnSubmit()
        {
            base.OnSubmit();
            _onSubmitAsObservableSubject.OnNext(Unit.Default);
            RectTransform.localScale = Vector3.one * SubmitScale;
        }

        public override void OnFocus()
        {
            _focused.SetActive(true);
            _unfocused.SetActive(false);
        }

        public override void OnUnFocus()
        {
            _focused.SetActive(false);
            _unfocused.SetActive(true);
        }

        private void Update()
        {
            RectTransform.localScale = Vector3.Lerp(RectTransform.localScale, Vector3.one, LerpT);
        }
    }
}
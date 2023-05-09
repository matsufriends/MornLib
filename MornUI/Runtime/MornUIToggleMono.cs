using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornUI
{
    public sealed class MornUIToggleMono : MornUISelectableMonoBase
    {
        [Header("Toggle")]
        [SerializeField] private bool _isOn;
        [SerializeField] private Graphic _checkMark;
        private readonly Subject<bool> _toggleChangeSubject = new();
        public IObservable<bool> OnToggleChanged => _toggleChangeSubject;

        protected override void OnValidateImpl()
        {
            if (_checkMark != null)
            {
                _checkMark.enabled = _isOn;
            }
        }

        public override void Submit()
        {
            _isOn = !_isOn;
            _checkMark.enabled = _isOn;
            _toggleChangeSubject.OnNext(_isOn);
        }
    }
}

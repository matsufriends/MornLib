using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MornUI
{
    public sealed class MornUIToggleMono : MornUIBehaviourMonoBase
    {
        [Header("Toggle")]
        [SerializeField] private bool _isOn;
        [SerializeField] private Graphic _checkMark;
        private readonly Subject<bool> _toggleChangeSubject = new();
        public IObservable<bool> OnToggleChanged => _toggleChangeSubject;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (_checkMark != null)
            {
                _checkMark.enabled = _isOn;
            }
        }

        public override void Selected()
        {
        }

        public override void OnSubmit(out bool canTransition)
        {
            ApplyValue(!_isOn);
            _toggleChangeSubject.OnNext(_isOn);
            canTransition = true;
        }

        public override void OnMove(MornUIAxisDirType axis, out bool canTransition)
        {
            canTransition = true;
        }

        public void ApplyValue(bool isOn)
        {
            _isOn = isOn;
            _checkMark.enabled = _isOn;
        }
    }
}

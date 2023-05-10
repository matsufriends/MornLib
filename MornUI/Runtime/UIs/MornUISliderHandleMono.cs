using System;
using UniRx;
using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class MornUISliderHandleMono : MornUIBehaviourMonoBase
    {
        [SerializeField] private RectTransform _rectTransform;
        private readonly Subject<MornUIAxisDirType> _handleMoveSubject = new();
        internal IObservable<MornUIAxisDirType> OnHandleMove => _handleMoveSubject;

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        internal void SetAnchoredPosition(Vector2 anchoredPosition)
        {
            _rectTransform.localPosition = anchoredPosition;
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
            _handleMoveSubject.OnNext(axis);
            canTransition = false;
        }
    }
}

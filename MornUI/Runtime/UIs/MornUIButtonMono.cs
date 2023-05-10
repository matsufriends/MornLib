using System;
using UniRx;
using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(MornUISelectableMono))]
    public sealed class MornUIButtonMono : MornUIBehaviourMonoBase
    {
        private readonly Subject<Unit> _clickedSubject = new();
        public IObservable<Unit> OnClicked => _clickedSubject;

        public override void Selected()
        {
        }

        public override void OnSubmit(out bool canTransition)
        {
            _clickedSubject.OnNext(Unit.Default);
            canTransition = true;
        }

        public override void OnMove(MornUIAxisDirType axis, out bool canTransition)
        {
            canTransition = true;
        }
    }
}

using System;
using UniRx;

namespace MornUI
{
    public sealed class MornUIButtonMono : MornUISelectableMonoBase
    {
        private readonly Subject<Unit> _clickedSubject = new();
        public IObservable<Unit> OnClicked => _clickedSubject;

        public override void Submit()
        {
            _clickedSubject.OnNext(Unit.Default);
        }
    }
}

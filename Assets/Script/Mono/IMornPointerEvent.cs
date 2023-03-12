using System;
using UniRx;

namespace MornLib.Mono
{
    public interface IMornPointerEvent
    {
        public IObservable<Unit> OnPointerEnter { get; }
        public IObservable<Unit> OnPointerExit { get; }
        public IObservable<MouseClickSet> OnPointerUp { get; }
        public IObservable<MouseClickSet> OnPointerDown { get; }
        public IObservable<MouseClickSet> OnPointerClick { get; }
    }
}

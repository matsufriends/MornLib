using System;
using UniRx;
namespace MornLib.Mono {
    public interface IPointerEvent {
        public IObservable<Unit> OnPointerEnter { get; }
        public IObservable<Unit> OnPointerExit { get; }
        public IObservable<Unit> OnPointerUp { get; }
        public IObservable<Unit> OnPointerDown { get; }
        public IObservable<Unit> OnPointerClick { get; }
    }
}
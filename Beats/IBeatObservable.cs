using System;
using UniRx;

namespace MornLib.Beats
{
    public interface IBeatObservable
    {
        IObservable<int> OnBeat { get; }
        IObservable<Unit> OnEndBeat { get; }
        float LeftTime { get; }
    }
}

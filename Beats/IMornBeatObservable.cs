using System;
using UniRx;

namespace MornLib.Beats
{
    public interface IMornBeatObservable
    {
        IObservable<int> OnBeat { get; }
        IObservable<Unit> OnEndBeat { get; }
        float LeftTime { get; }
    }
}

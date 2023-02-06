using System;
using UniRx;

namespace MornLib.Beats
{
    public interface IMornBeatObservable
    {
        IObservable<BeatTimingInfo> OnBeat { get; }
        IObservable<Unit> OnEndBeat { get; }
        float LeftMeasureTime { get; }
    }
}

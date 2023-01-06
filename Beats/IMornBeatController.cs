using System;

namespace MornLib.Beats
{
    public interface IMornBeatController<TEnum> where TEnum : Enum
    {
        void MyUpdate(float time);
        void BeatStart(TEnum beatType);
    }
}

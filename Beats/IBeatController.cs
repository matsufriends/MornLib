using System;

namespace MornLib.Beats
{
    public interface IBeatController<TEnum> where TEnum : Enum
    {
        void MyUpdate(float time);
        void BeatStart(TEnum beatType);
    }
}

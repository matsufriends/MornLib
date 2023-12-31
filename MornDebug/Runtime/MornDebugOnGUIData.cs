using System;

namespace MornDebug
{
    public sealed class MornDebugOnGUIData : IDisposable
    {
        public readonly string Label;
        public readonly Action OnGUI;
        public bool IsDisposed { get; private set; }

        public MornDebugOnGUIData(string label, Action onGUI)
        {
            Label = label;
            OnGUI = onGUI;
        }

        void IDisposable.Dispose()
        {
            IsDisposed = true;
        }
    }
}
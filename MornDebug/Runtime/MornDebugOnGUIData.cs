using System;

namespace MornDebug
{
    public sealed class MornDebugOnGUIData : IDisposable
    {
        public readonly string Label;
        public readonly Action OnGUI;

        public MornDebugOnGUIData(string label, Action onGUI)
        {
            Label = label;
            OnGUI = onGUI;
        }

        public bool IsDisposed { get; private set; }

        void IDisposable.Dispose()
        {
            IsDisposed = true;
        }
    }
}
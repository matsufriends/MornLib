using System;

namespace MornEditor
{
    public sealed class MornEditorOnGUIData : IDisposable
    {
        public readonly string Label;
        public readonly Action OnGUI;
        public bool IsDisposed { get; private set; }

        public MornEditorOnGUIData(string label, Action onGUI)
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
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MornEditor.Editor")]
namespace MornEditor
{
    public static class MornEditorCore
    {
        internal static readonly HashSet<MornEditorOnGUIData> OnGUIHashSet = new();
        private static readonly List<MornEditorOnGUIData> _cacheList = new();

        public static void RegisterOnGUI(string label, Action action)
        {
            OnGUIHashSet.Add(new MornEditorOnGUIData(label, action));
        }

        internal static void CheckDisposed()
        {
            foreach (var info in OnGUIHashSet)
            {
                if (info.IsDisposed)
                {
                    _cacheList.Add(info);
                }
            }

            foreach (var info in _cacheList)
            {
                OnGUIHashSet.Remove(info);
            }

            _cacheList.Clear();
        }
    }
}
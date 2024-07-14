using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MornDebug.Editor")]

namespace MornDebug
{
    public static class MornDebugCore
    {
        internal static readonly HashSet<MornDebugOnGUIData> OnGUIHashSet = new();
        private static readonly List<MornDebugOnGUIData> _cacheList = new();

        public static void RegisterOnGUI(string label, Action action)
        {
            OnGUIHashSet.Add(new MornDebugOnGUIData(label, action));
        }

        internal static void CheckDisposed()
        {
            foreach (var info in OnGUIHashSet)
                if (info.IsDisposed)
                    _cacheList.Add(info);

            foreach (var info in _cacheList) OnGUIHashSet.Remove(info);

            _cacheList.Clear();
        }
    }
}
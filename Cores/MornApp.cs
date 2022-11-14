using System;
using System.Collections.Generic;
using UniRx;
namespace MornLib.Cores {
    public static class MornApp {
        private static readonly CompositeDisposable s_disposable = new();
        public static ICollection<IDisposable> QuitDisposable => s_disposable;
        public static void Quit() {
            s_disposable.Clear();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            UnityEngine.Application.Quit();
            #endif
        }
    }
}
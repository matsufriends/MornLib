using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEditor;

namespace MornLib.Cores
{
    public static class MornApp
    {
        private static readonly CancellationTokenSource s_tokenSource = new();
        private static readonly CompositeDisposable s_disposable = new();
        public static CancellationToken QuitToken => s_tokenSource.Token;
        public static ICollection<IDisposable> QuitDisposable => s_disposable;
        

        public static void Quit()
        {
            s_disposable.Clear();
            s_tokenSource.Cancel();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
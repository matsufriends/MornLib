using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MornLib.Cores
{
    public static class MornApp
    {
        private static readonly CancellationTokenSource TokenSource = new();
        private static readonly CompositeDisposable Disposable = new();
        public static CancellationToken QuitToken => TokenSource.Token;
        public static ICollection<IDisposable> QuitDisposable => Disposable;

        static MornApp()
        {
            var flag = new GameObject(nameof(MornAppFlag));
            flag.AddComponent<MornAppFlag>();
            Object.DontDestroyOnLoad(flag);
            flag.OnDestroyAsObservable().Subscribe(x =>
            {
                Disposable.Clear();
                TokenSource.Cancel();
            });
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
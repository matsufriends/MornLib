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
        private static CancellationTokenSource _tokenSource;
        private static CompositeDisposable _disposable;
        private static GameObject _flag;

        public static CancellationToken QuitToken
        {
            get
            {
                if (_flag == null) GenerateFlag();

                return _tokenSource.Token;
            }
        }

        public static ICollection<IDisposable> QuitDisposable
        {
            get
            {
                if (_flag == null) GenerateFlag();

                return _disposable;
            }
        }

        private static void GenerateFlag()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            _disposable?.Clear();
            _disposable = new CompositeDisposable();
            _flag = new GameObject(nameof(MornAppFlag));
            _flag.AddComponent<MornAppFlag>();
            Object.DontDestroyOnLoad(_flag);
            _flag.OnDestroyAsObservable()
                .Subscribe(x =>
                {
                    _tokenSource.Cancel();
                    _disposable.Clear();
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
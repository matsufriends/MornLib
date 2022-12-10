using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornLib.Cores;
using MornLib.Singletons;
using UniRx;
using UnityEngine;

namespace MornLib.Frames
{
    public class MornFrameManagerMono : MornSingletonMono<MornFrameManagerMono>
    {
        private readonly Subject<Unit> _updateSubject = new();
        private MornTaskCanceller _canceller;
        private float _cachedUpdateTime;
        private float _frameTime;
        public IObservable<Unit> OnUpdate => _updateSubject;

        protected override void MyAwake()
        {
            _canceller = new MornTaskCanceller(gameObject);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 9999;
        }

        public void StartUpdate()
        {
            _canceller.Cancel();
            MyUpdateAsync(_canceller.Token).Forget();
        }

        private async UniTaskVoid MyUpdateAsync(CancellationToken token)
        {
            while (true)
            {
                var current = Time.realtimeSinceStartup;
                var frameSec = 1f / Mathf.Max(1, MornFrameSettingSo.Instance.AimFps);
                while (_frameTime + frameSec <= current)
                {
                    _frameTime += frameSec;
                    _updateSubject.OnNext(Unit.Default);
                }

                await UniTask.WaitForEndOfFrame(this, token);
            }
        }
    }
}

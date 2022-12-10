using System;
using System.Collections;
using System.Threading;
using MornLib.Singletons;
using UniRx;
using UnityEngine;

namespace MornLib.Frames
{
    public class MornFrameManagerMono : MornSingletonMono<MornFrameManagerMono>
    {
        private bool _isUpdating;
        private float _currentFrameTime;
        private readonly Subject<Unit> _updateSubject = new();
        public IObservable<Unit> OnUpdate => _updateSubject;

        protected override void MyAwake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 9999;
        }

        public void StartUpdate()
        {
            if (_isUpdating)
            {
                return;
            }

            _isUpdating = true;
            StartCoroutine(WaitForNextFrame());
        }

        private void Update()
        {
            if (_isUpdating)
            {
                _updateSubject.OnNext(Unit.Default);
            }
        }

        private IEnumerator WaitForNextFrame()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _currentFrameTime += 1f / MornFrameSettingSo.Instance.AimFps;
                var realTime = Time.realtimeSinceStartup;

                //フレーム秒以上にリアルタイムが進んでいるとき、フレーム秒を補正
                if (_currentFrameTime < realTime)
                {
                    _currentFrameTime = realTime;
                }

                //フレーム秒まで0.01秒以上残っていれば、フレーム秒の0.01秒前までThread.Sleep
                if (realTime + 0.01f < _currentFrameTime)
                {
                    var sleepTime = _currentFrameTime - realTime - 0.01f;
                    Thread.Sleep((int)(sleepTime * 1000));
                }

                //フレーム秒まで待機(0.01秒未満)
                while (realTime < _currentFrameTime)
                {
                    realTime = Time.realtimeSinceStartup;
                }
            }
        }
    }
}

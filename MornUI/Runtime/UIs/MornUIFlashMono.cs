using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MornUI
{
    public sealed class MornUIFlashMono : MonoBehaviour
    {
        [SerializeField] private Graphic _graphic;
        [SerializeField] private float _waitFlashSeconds = 1;
        [SerializeField] private int _flashCount = 4;
        [SerializeField] private float _flashSeconds = 0.1f;
        [SerializeField] private float _waitSeconds = 0.3f;
        private bool _isFlashLock;

        private void Update()
        {
            if (_isFlashLock == false)
            {
                _graphic.SetAlpha(Mathf.PingPong(2 * Time.unscaledTime / _waitFlashSeconds, 1));
            }
        }

        public async UniTask FlashAsync(CancellationToken token)
        {
            _isFlashLock = true;
            var elapsedTime = 0f;
            while (elapsedTime < _flashCount * _flashSeconds)
            {
                elapsedTime += Time.deltaTime;
                _graphic.SetAlpha(1 - Mathf.PingPong(2 * elapsedTime / _flashSeconds, 1));
                await UniTask.NextFrame(token);
            }

            _graphic.SetAlpha(1);
            await UniTask.Delay(TimeSpan.FromSeconds(_waitSeconds), cancellationToken: token);
            _isFlashLock = false;
        }
    }
}

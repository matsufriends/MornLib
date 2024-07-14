using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MornLib.Cores;
using UnityEngine;

namespace MornTween
{
    [AddComponentMenu("")]
    public sealed class MornTweenCanvasGroupCtrl : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private CancellationTokenSource cts;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public async UniTask FadeFillAsync(TimeSpan duration, bool isUnscaled = false,
            CancellationToken cancellationToken = default)
        {
            cts?.Cancel();
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await MornTask.TransitionAsync(duration, canvasGroup.alpha, 1, rate => canvasGroup.alpha = rate, isUnscaled,
                cts.Token);
        }

        public async UniTask FadeClearAsync(TimeSpan duration, bool isUnscaled = false,
            CancellationToken cancellationToken = default)
        {
            cts?.Cancel();
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await MornTask.TransitionAsync(duration, canvasGroup.alpha, 0, rate => canvasGroup.alpha = rate, isUnscaled,
                cts.Token);
        }
    }
}
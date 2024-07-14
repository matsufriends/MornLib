using System.Threading;
using Cysharp.Threading.Tasks;
using MornLib.Cores;
using UnityEngine;

namespace MornTween
{
    public static class MornTweenCore
    {
        private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out var component) ? component : gameObject.AddComponent<T>();
        }

        public static async UniTask FadeFillAsync(this CanvasGroup target, float duration, bool isUnscaled = false,
            CancellationToken cancellationToken = default)
        {
            var mornTweenCanvasGroup = GetOrAddComponent<MornTweenCanvasGroupCtrl>(target.gameObject);
            await mornTweenCanvasGroup.FadeFillAsync(duration.ToTimeSpanAsSeconds(), isUnscaled, cancellationToken);
        }

        public static async UniTask FadeClearAsync(this CanvasGroup target, float duration, bool isUnscaled = false,
            CancellationToken cancellationToken = default)
        {
            var mornTweenCanvasGroup = GetOrAddComponent<MornTweenCanvasGroupCtrl>(target.gameObject);
            await mornTweenCanvasGroup.FadeClearAsync(duration.ToTimeSpanAsSeconds(), isUnscaled, cancellationToken);
        }
    }
}
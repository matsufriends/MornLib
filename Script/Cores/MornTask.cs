using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib.Cores
{
    public static class MornTask
    {
        public async static UniTask TransitionAsync(TimeSpan duration, float startValue, float endValue, Action<float> action, bool useUnscaledTime = false, CancellationToken cancellationToken = default)
        {
            await TransitionAsync(duration, x => action(Mathf.Lerp(startValue, endValue, x)), useUnscaledTime, cancellationToken);
        }

        public async static UniTask TransitionAsync(TimeSpan duration, Action<float> action, bool useUnscaledTime = false, CancellationToken cancellationToken = default)
        {
            var totalSeconds = (float)duration.TotalSeconds;
            if (totalSeconds < 0)
            {
                throw new ArgumentException("durationは0以上の値を指定して下さい。");
            }

            if (totalSeconds == 0)
            {
                action(1);
                return;
            }

            var startTime = useUnscaledTime ? Time.unscaledTime : Time.time;
            while (true)
            {
                var elapsedTime = (useUnscaledTime ? Time.unscaledTime : Time.time) - startTime;
                action(Mathf.Clamp01(elapsedTime / totalSeconds));
                if (elapsedTime >= totalSeconds)
                {
                    break;
                }

                await UniTask.NextFrame(cancellationToken);
            }
            action(1f);
        }
    }
}